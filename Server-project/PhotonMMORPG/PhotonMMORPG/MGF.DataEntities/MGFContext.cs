using MGF.DataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF
{
#if MYSQL
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MGFContext : DbContext
    {

        public MGFContext() : base("name=MsqlDbConnectionString")
        {

        }
#else
    public class MGFContext : DbContext
    {

        public MGFContext() : base("name=MsSqlDbConnectionString")
        {

        }
#endif
        //Define entities Here
        public DbSet<Character> Characters { get; set; }
        public DbSet<Stat> Stats { get; set; }
    }

}
