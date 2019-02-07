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

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Stat> Stats { get; set; }
    }
}
