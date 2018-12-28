using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGF.DataEntities;

namespace MGF
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (MGFContext entities = new MGFContext())
            {
                Character character = new Character() { Name = "The dude" };
                entities.Characters.Add(character);
                entities.SaveChanges();
            }
        }
    }
}
