using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF.DataEntities
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public decimal ExperiencePoints { get; set; }
        public float LifePoints { get; set; }
        public float ManaPoints { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Stat> Stats { get; set; }

        public float Loc_x { get; set; }
        public float Loc_y { get; set; }
        public float Loc_z { get; set; }
        public float Rot_x { get; set; }
        public float Rot_y { get; set; }
        public float Rot_z { get; set; }
    }
}
