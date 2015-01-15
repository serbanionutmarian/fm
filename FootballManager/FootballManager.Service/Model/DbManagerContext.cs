using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class DbManagerContext : DbContext
    {
        public DbManagerContext()
            : base("Name=DbManagerContext")
        {
          
        }
        public void SetMultipleActiveResultSets() {
            // to do (must investigate) ("there is already an open datareader associated with this command which must be closed first. ");
            this.Database
            .Connection
            .ConnectionString = this.Database.Connection.ConnectionString + ";MultipleActiveResultSets=true;";
        }


        public DbSet<Country> Countries { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<LeagesConfiguration> LeagesConfigurations { get; set; }

        public DbSet<Series> Series { get; set; }

        public DbSet<PlayerAttributeValue> PlayersAttributesValues { get; set; }

        public DbSet<DataModel.Tables.Player> Players { get; set; }


        public DbSet<Match> Matches { get; set; }
    }
}
