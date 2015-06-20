using Repository;
using Repository.Interfaces;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto.Auth.Request;
using Dto.Auth.Response;
using Dto;

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

        public SignupResponse SignUp(SignupRequest request)
        {
            return new SignupResponse()
            {
                Result = request.DisplayName + "..hmm"
            };
            // TO DO!! (when we'll add countries in UI)
            if (request.CountryId == 0)
            {
                // romania
                request.CountryId = 1;
            }
            // TO DO!! (transaction must be added)
            var team = _teamRepository.GetRandomBoot(request.CountryId);
            if (team == null)
            {
                var error = "There is no team to take in this country!! Please select another country or wait to the end of the month!!";
                return ResponseBase.CreateValidationError<SignupResponse>(error);
            }
            team.IsBoot = false;
            _userRepository.Add(new DataModel.Tables.User()
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                Password = request.Password,
                CountryId = request.CountryId,
                Team = team,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _unitOfWork.SaveChanges();

            return null;
        }


        public bool TryAuthenticate(string email, string password, out DataModel.Tables.User user)
        {
            user = _userRepository.GetBy(email, password);
            return user != null;
        }
    }
}
