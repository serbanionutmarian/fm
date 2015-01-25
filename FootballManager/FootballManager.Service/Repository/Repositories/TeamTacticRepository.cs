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
    public class TeamTacticRepository : GenericRepository<TeamTactic>, ITeamTacticRepository
    {
        public TeamTacticRepository(DbContext context)
            : base(context)
        {

        }
    }
}
