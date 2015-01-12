using DataModel.Tables;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(DbContext context)
            : base(context)
        {

        }

        public Team GetRandomBoot(int countryId)
        {
            var parameters = new List<object>();
            parameters.Add(new SqlParameter("countryId", countryId));
            return _entities.Database.SqlQuery<Team>("getRandomBoot @countryId", parameters.ToArray()).SingleOrDefault();
        }
    }
}
