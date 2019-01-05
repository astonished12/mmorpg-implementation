using MGF;
using MGF.DataEntities;

namespace DataBaseCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MGFContext entities = new MGFContext())
            {
                entities.Users.Add(new User() {LoginName = "Mucu"});
                entities.SaveChanges();
            }
        }
    }
}
