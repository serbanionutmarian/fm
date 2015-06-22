using Dto;
using Dto.Request;
using Dto.Response;
using ServiceCaller.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCaller.Services
{
    public class AuthService : ServiceBase
    {
        public SignupResponse SignUp(SignupRequest request)
        {
            return _caller.SignUp(request);
        }

        public bool IsLogIn()
        {
            return _caller.IsLogIn();
        }
    }
}
