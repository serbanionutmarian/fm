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
    public class TeamService : EntityService<Team>, ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;

        public TeamService(IUnitOfWork unitOfWork, ITeamRepository teamRepository)
            : base(unitOfWork, teamRepository)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
        }
    }
}
