using Photon.MmoDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGF_Photon.Implementation.Data;
using MultiplayerGameFramework.Interfaces.Config;
using MultiplayerGameFramework.Interfaces.Server;
using Servers.Config;

namespace Servers.Models
{
    public class GridWorld
    {
        private readonly Region[][] worldRegions;

        public GridWorld(BoundingBox area, Vector tileDimensions)
        {
            // 2D grid: extend Z to max possible
            this.Area = area;
            this.TileDimensions = tileDimensions;
            this.TileX = (int)Math.Ceiling(Area.Size.X / (double)tileDimensions.X);
            this.TileY = (int)Math.Ceiling(Area.Size.Y / (double)tileDimensions.Y);

            this.worldRegions = new Region[TileX][];
            int totalRegion = 0;
            for (int x = 0; x < TileX; x++)
            {
                this.worldRegions[x] = new Region[TileY];
                for (int y = 0; y < TileY; y++)
                {
                    this.worldRegions[x][y] = new Region(x, y) { Name = "Region " + totalRegion, ZoneId = Guid.NewGuid() };
                    totalRegion += 1;

                }
            }
        }

        ~GridWorld()
        {
        }

        public BoundingBox Area { get; private set; }

        public Vector TileDimensions { get; private set; }

        public int TileX { get; private set; }
        public int TileY { get; private set; }



        public Region GetRegion(Vector position)
        {
            Vector p = position - this.Area.Min;
            if (p.X >= 0 && p.X < Area.Size.X && p.Y >= 0 && p.Y < Area.Size.Y)
            {
                int x = (int)(p.X / this.TileDimensions.X);
                int y = (int)(p.Y / this.TileDimensions.Y);
                return this.worldRegions[x][y];
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Region> GetRegions(BoundingBox area)
        {
            return GetRegionsEnumerable(area).ToArray();
        }

        private IEnumerable<Region> GetRegionsEnumerable(BoundingBox area)
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
                    yield return this.worldRegions[x][y];
                }
            yield break;
        }

        public Region[][] GetAllRegions()
        {
            return this.worldRegions;
        }

        public void SetRegionsToServers(IServerConnectionCollection<IServerType, IServerPeer> serverConnectionCollection)
        {
            var regionServers = serverConnectionCollection.GetServersByType<IServerPeer>(ServerType.RegionServer).ToList();
            for (int i = 0; i <= worldRegions.GetUpperBound(0); i++)
            {
                var srvApplicationName = regionServers[i].ServerData<ServerData>().ApplicationName;
           
                foreach (var region in worldRegions[i])
                {
                    region.ApplicationServerName = srvApplicationName;
                }
            }


        }
    }
}

