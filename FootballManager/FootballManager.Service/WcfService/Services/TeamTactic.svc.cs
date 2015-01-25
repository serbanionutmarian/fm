using DataModel;
using DataModel.Tables;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService.Interfaces;

namespace WcfService.Services
{
    public class TeamTactic : ITeamTactic
    {
        private readonly ITeamTacticService _teamTacticService;

        public TeamTactic(ITeamTacticService teamTacticService)
        {
            _teamTacticService = teamTacticService;
        }

        public DataModel.Tables.TeamTactic GetById(int teamId)
        {
            return _teamTacticService.GetById(teamId);
        }

        public void Update(DataModel.Tables.TeamTactic teamTactic)
        {
            _teamTacticService.Update(teamTactic);
        }
    }
}
