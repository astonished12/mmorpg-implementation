using System.Collections.Generic;
using Servers.Models.Interfaces;

namespace Servers.Models.Templates
{
    public class NpcTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prefab { get; set; }
        public object Type { get; set; }
        public int Respawn { get; set; }
        public object AiType { get; set; }
        public List<ItemDrop> DropList { get; set; }
        public Dictionary<IStat, float> Stats { get; set; }
        public float WidthRadius { get; set; }
    }
}