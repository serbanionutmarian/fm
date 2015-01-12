using Repository;
using Repository.Interfaces;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoModel.Auth;
using System.Activities.Statements;

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

        public void SignUp(SignupDto input)
        {
            // TO DO!! (when we'll add countries in UI)
            if (input.CountryId == 0)
            {
                // romania
                input.CountryId = 1;
            }
            // TO DO!! (transaction must be added)
            var team = _teamRepository.GetRandomBoot(input.CountryId);
            if (team == null)
            {
                throw new Exception("There is no team to take in this country!! Please select another country or wait to the end of the month!!");
            }
            team.IsBoot = false;
            _userRepository.Add(new DataModel.Tables.User()
            {
                DisplayName = input.DisplayName,
                Email = input.Email,
                Password = input.Password,
                CountryId = input.CountryId,
                Team = team,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _unitOfWork.SaveChanges();
        }
    }
}
