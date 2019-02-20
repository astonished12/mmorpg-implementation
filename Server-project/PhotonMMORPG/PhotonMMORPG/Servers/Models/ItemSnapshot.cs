using Photon.MmoDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Models
{
    public class ItemSnapshot
    {
        public ItemSnapshot(Item source, Vector position, Vector rotation, Region worldRegion, int propertiesRevision)
        {
            this.Source = source;
            this.Position = position;
            this.Rotation = rotation;
            this.PropertiesRevision = propertiesRevision;
        }

        public Item Source { get; private set; }
        public Vector Position { get; private set; }
        public Vector Rotation { get; private set; }
        public int PropertiesRevision { get; private set; }

    }
}
