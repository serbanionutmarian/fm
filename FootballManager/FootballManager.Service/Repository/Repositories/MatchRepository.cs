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
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        public MatchRepository(DbContext context)
            : base(context)
        {

        }

        public void DeleteAll()
        {
            _entities.Database.ExecuteSqlCommand("DELETE * FROM Matches"); 
        }
    }
}
