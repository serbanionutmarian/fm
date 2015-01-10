using Repository;
using Repository.Interfaces;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoModel.Auth;

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

            _userRepository.Add(new DataModel.Tables.User()
            {
                DisplayName = input.DisplayName,
                Email = input.Email,
                Password = input.Password,
                CountryId = input.CountryId,
                TeamId = _teamRepository.GetBestBootId(input.CountryId),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _unitOfWork.Commit();
        }
    }
}
