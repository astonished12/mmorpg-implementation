using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCommon.SerializedObjects
{
    [Serializable]
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public decimal ExperiencePoints { get; set; }
        public int LifePoints { get; set; }
        public int ManaPoints { get; set; }
        public float Loc_X { get; set; }
        public float Loc_Y { get; set; }
        public float Loc_Z { get; set; }
    }
}
