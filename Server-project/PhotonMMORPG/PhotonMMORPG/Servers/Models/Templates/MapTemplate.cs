using System.Collections.Generic;
using Photon.MmoDemo.Common;

namespace Servers.Models.Templates
{
    public class MapTemplate
    {
        public List<Vector> Spawns { get; set; }

        public MapTemplate()
        {
            Spawns = new List<Vector>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string SceneName { get; set; }
        public Vector StartPosition { get; set; }
        public float ZoneHeight { get; set; }
        public float ZoneLength { get; set; }
    }
}
