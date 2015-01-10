using Model.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DbManagerContext : DbContext
    {
        public DbManagerContext()
            : base("Name=DbManagerContext")
        {

        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<LeagesConfiguration> LeagesConfigurations { get; set; }

        public DbSet<Series> Series { get; set; }
    }
}
