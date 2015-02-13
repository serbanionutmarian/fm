using DataModel.Tables;
using Dto.Auth.Request;
using Dto.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Interfaces
{
    public interface IAuthService
    {
        SignupResponse SignUp(SignupRequest request);

        bool TryAuthenticate(string userName, string password, out User user);
    }
}
