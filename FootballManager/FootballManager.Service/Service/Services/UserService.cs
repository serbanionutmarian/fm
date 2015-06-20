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
    public class UserService : EntityService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
            : base(unitOfWork, userRepository)
        {
            _userRepository = userRepository;
        }
        public User GetById(int userId)
        {
            return _userRepository.GetById(userId);
        }
    }
}
