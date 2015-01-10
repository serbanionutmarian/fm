using DataModel.Tables;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public int GetBestBootId(int countryId)
        {
            // TO DO!! (it's possible to work slow)
            var result = _entities.Database.SqlQuery<int>("getBestBootId", countryId);
            return result.Single();
        }
    }
}
