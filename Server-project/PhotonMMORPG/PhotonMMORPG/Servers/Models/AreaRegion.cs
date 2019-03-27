using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerGameFramework.Interfaces.Server;
using Photon.SocketServer.Concurrency;
using Servers.Models.Interfaces;
using StackExchange.Redis;

namespace Servers.Models
{

    /// <summary>
    /// Item notifies interest areas via regions this item exits and enters.
    /// </summary>
    public class ItemRegionChangedMessage
    {
        public ItemRegionChangedMessage(AreaRegion r0, AreaRegion r1, ItemSnapshot snaphot)
        {
            this.Region0 = r0;
            this.Region1 = r1;
            this.ItemSnapshot = snaphot;
        }
        public AreaRegion Region0 { get; private set; }
        public AreaRegion Region1 { get; private set; }
        public ItemSnapshot ItemSnapshot { get; private set; }
    };


    public class AreaRegion : IAreaRegion
    {
        public AreaRegion(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        // grid cell X (debug only)
        public int X { get; private set; }

        // grid cell Y (debug only)
        public int Y { get; private set; }

        public string Name { get; set; }

        public Guid ZoneId { get; set; }

        public IWorld World { get; set; }

        public int GameTick { get; set; }

        public string ApplicationServerName { get; set; }


        public override string ToString()
        {
            return string.Format("AreaAreaRegion({0},{1})", base.ToString(), X, Y);
        }

        public void AddObject(IObject obj)
        {
            throw new NotImplementedException();
        }

        public void RemoveObject(IObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
