using DataModel.Tables;
using DataService.Interfaces;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Services
{
    public class TeamTacticService : EntityService<TeamTactic>, ITeamTacticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamTacticRepository _teamTacticRepository;

        public TeamTacticService(IUnitOfWork unitOfWork, ITeamTacticRepository teamTacticRepository)
            : base(unitOfWork, teamTacticRepository)
        {
            _unitOfWork = unitOfWork;
            _teamTacticRepository = teamTacticRepository;
        }

        public TeamTactic GetById(int teamId)
        {
            return _teamTacticRepository.FindBy(tt => tt.TeamId == teamId).Single();
        }
    }
}
