using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer.Concurrency;
using Servers.Models.Interfaces;

namespace Servers.Models
{

    /// <summary>
    /// Item notifies interest areas via regions this item exits and enters.
    /// </summary>
    public class ItemRegionChangedMessage
    {
        public ItemRegionChangedMessage(Region r0, Region r1, ItemSnapshot snaphot)
        {
            this.Region0 = r0;
            this.Region1 = r1;
            this.ItemSnapshot = snaphot;
        }
        public Region Region0 { get; private set; }
        public Region Region1 { get; private set; }
        public ItemSnapshot ItemSnapshot { get; private set; }
    };


    public class Region : IRegion
    {
        public Region(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        // grid cell X (debug only)
        public int X { get; private set; }

        // grid cell Y (debug only)
        public int Y { get; private set; }

        public string Name => throw new NotImplementedException();

        public Guid ZoneId => throw new NotImplementedException();

        public IWorld World => throw new NotImplementedException();

        public int GameTick => throw new NotImplementedException();


        public override string ToString()
        {
            return string.Format("Region({0},{1})", base.ToString(), X, Y);
        }

        public void Dispose()
        {
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
