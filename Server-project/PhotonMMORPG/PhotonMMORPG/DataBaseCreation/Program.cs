using System.Data.Entity;
using MGF;
using MGF.DataEntities;
using System.Linq;

namespace DataBaseCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MGFContext entities = new MGFContext())
            {
                var entity = entities.Users.AsQueryable()
                    .Include(y => y.Characters).FirstOrDefault(t => t.Id == 2);
                var x = "moe";

                //var character1 = new Character() { Name = "Elefantu", UserId = entity.Id };
                //var character2 = new Character() { Name = "Elefanta", UserId = entity.Id };

                //entities.Characters.Add(character1);
                //entities.Characters.Add(character2);


                //entities.SaveChanges();

            }
        }
    }
}
