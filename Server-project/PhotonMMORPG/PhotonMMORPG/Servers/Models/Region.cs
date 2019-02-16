using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Models
{
    public class Region
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

    }
}
