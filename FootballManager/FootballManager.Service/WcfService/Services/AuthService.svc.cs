using DtoModel.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService.Interfaces;

namespace WcfService.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataService.Interfaces.IAuthService _service;

        public AuthService(DataService.Interfaces.IAuthService service)
        {
            _service = service;
        }

        public void SignUp(SignupDto input)
        {
            _service.SignUp(input);
        }
    }
}
