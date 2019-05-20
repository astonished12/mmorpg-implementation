using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCommon.SerializedObjects
{
    [Serializable]
    public class Character : ICharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public decimal ExperiencePoints { get; set; }
        public float LifePoints { get; set; }
        public float ManaPoints { get; set; }
        public float Loc_X { get; set; }
        public float Loc_Y { get; set; }
        public float Loc_Z { get; set; }

        public float Rot_X { get; set; }
        public float Rot_Y { get; set; }
        public float Rot_Z { get; set; }
        public float Speed { get; set; }
        public bool Jump { get; set; }
        public bool Die { get; set; }
        public bool Respawn { get; set; }
        public bool Attack { get; set; }
    }

    public interface ICharacter
    {
    }
}
