using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Interfaces
{
    public interface ITeamTacticService : IEntityService<TeamTactic>
    {
        TeamTactic GetById(int teamId);
    }
}
