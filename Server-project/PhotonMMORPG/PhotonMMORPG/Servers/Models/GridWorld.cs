using Photon.MmoDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MGF_Photon.Implementation.Data;
using MultiplayerGameFramework.Implementation.Messaging;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using Photon.SocketServer;
using Servers.Config;
using Servers.Models.Factories;
using Servers.Models.Interfaces;
using ServiceStack;

namespace Servers.Models
{
    public class GridWorld
    {
        private readonly AreaRegion[][] _worldAreaRegions;

        public GridWorld(BoundingBox area, Vector tileDimensions)
        {
            // 2D grid: extend Z to max possible
            this.Area = area;
            this.TileDimensions = tileDimensions;
            this.TileX = (int)Math.Ceiling(Area.Size.X / (double)tileDimensions.X);
            this.TileY = (int)Math.Ceiling(Area.Size.Y / (double)tileDimensions.Y);

            this._worldAreaRegions = new AreaFactory().Create(this.TileX, this.TileY);
            
        }

        ~GridWorld()
        {
        }

        public BoundingBox Area { get; private set; }

        public Vector TileDimensions { get; private set; }

        public int TileX { get; private set; }
        public int TileY { get; private set; }



        public AreaRegion GetRegion(Vector position)
        {
            Vector p = position - this.Area.Min;
            if (p.X >= 0 && p.X < Area.Size.X && p.Y >= 0 && p.Y < Area.Size.Y)
            {
                int x = (int)(p.X / this.TileDimensions.X);
                int y = (int)(p.Y / this.TileDimensions.Y);
                return this._worldAreaRegions[x][y];
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<AreaRegion> GetRegions(BoundingBox area)
        {
            return GetRegionsEnumerable(area).ToArray();
        }

        private IEnumerable<AreaRegion> GetRegionsEnumerable(BoundingBox area)
        {
            BoundingBox overlap = this.Area.IntersectWith(area);
            var min = overlap.Min - this.Area.Min;
            var max = overlap.Max - this.Area.Min;
            // convert to tile coordinates and check bounds
            int x0 = Math.Max((int)(min.X / this.TileDimensions.X), 0);
            int x1 = Math.Min((int)Math.Ceiling(max.X / this.TileDimensions.X), TileX);
            int y0 = Math.Max((int)(min.Y / this.TileDimensions.Y), 0);
            int y1 = Math.Min((int)Math.Ceiling(max.Y / this.TileDimensions.Y), TileY);
            for (int x = x0; x < x1; x++)
                for (int y = y0; y < y1; y++)
                {
                    yield return this._worldAreaRegions[x][y];
                }
            yield break;
        }

        public AreaRegion[][] GetAllRegions()
        {
            return this._worldAreaRegions;
        }

        public void SetRegionsToServers(IServerConnectionCollection<IServerType, IServerPeer> serverConnectionCollection)
        {
            var regionServers = serverConnectionCollection.GetServersByType<IServerPeer>(ServerType.RegionServer).ToList();
            for (int i = 0; i <= _worldAreaRegions.GetUpperBound(0); i++)
            {
                var srvApplicationName = regionServers[i].ServerData<ServerData>().ApplicationName;

                foreach (var region in _worldAreaRegions[i])
                {
                    region.ApplicationServerName = srvApplicationName;
                }

                IMessage message = new Request((byte)MessageOperationCode.Region,
                    null,
                    new Dictionary<byte, object>()
                    {
                        { regionServers[i].Server.SubCodeParameterCode,(int) MessageSubCode.AssignAreaMap},
                        { (byte)MessageParameterCode.Object , MessageSerializerService.SerializeObjectOfType(_worldAreaRegions[i]) }
                    });

                regionServers[i].SendMessage(message);
            }

        }
    }
}

