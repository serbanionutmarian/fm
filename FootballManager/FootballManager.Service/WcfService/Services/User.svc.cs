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
    public class User : IUser
    {
        private readonly IUserService _userService;

        public User(IUserService userService)
        {
            _userService = userService;
        }
        public DataModel.Tables.User GetByUserId(int userId)
        {
            return _userService.GetById(userId);
        }
    }
}
