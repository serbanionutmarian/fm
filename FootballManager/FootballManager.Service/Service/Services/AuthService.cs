using Repository;
using Repository.Interfaces;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;

        public AuthService(IUnitOfWork unitOfWork, IUserRepository userRepository, ITeamRepository teamRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        public void SignUp(Model.Dto.SignupDto input)
        {
            _userRepository.Add(new Model.Tables.User()
            {
                DisplayName = input.DisplayName,
                Email = input.Email,
                Password = input.Password,
                CountryId = input.CountryId,
                TeamId = input.TeamId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            _teamRepository.Add(new Model.Tables.Team()
            {
                Name = input.TeamName
            });

            _unitOfWork.Commit();
        }
    }
}
