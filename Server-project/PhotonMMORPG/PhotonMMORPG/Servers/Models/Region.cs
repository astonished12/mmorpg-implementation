using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

 

    public class Region : IDisposable
    {
        private int coordX, coordY;
        private IList<Region> neighboursRegions;
        private bool active;
        private Dictionary<int, RegionObject> visibleRegionObjects;

        public Region(int coordX, int coordY)
        {
            this.coordX = coordX;
            this.coordY = coordX;
            visibleRegionObjects = new Dictionary<int, RegionObject>();
            Active = true;
        }

        public bool Active
        {
            get { return active; }
            set
            {
                if (active == value)
                    return;

                active = value;

                //switchAI(value);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
