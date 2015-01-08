using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService
{
    public class AuthService : IAuthService
    {
        private readonly DataService.Interfaces.IAuthService _service;

        public AuthService(DataService.Interfaces.IAuthService service)
        {
            _service = service;
        }

        public void SignUp(Model.Dto.SignupDto input)
        {
            _service.SignUp(input);
        }
    }
}
