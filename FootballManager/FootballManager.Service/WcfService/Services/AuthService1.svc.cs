using Dto.Auth.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.Text;
using WcfService.Interfaces;

namespace WcfService.Services
{
    //  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AuthService1 : IAuthService
    {
        //private readonly DataService.Interfaces.IAuthService _service;

        //public AuthService(DataService.Interfaces.IAuthService service)
        //{
        //    _service = service;
        //}

        public AuthService1()
        {
        }

        public void SignUp(SignupRequest input)
        {
            // _service.SignUp(input);
        }

        //   [PrincipalPermission(SecurityAction.Demand, Name = "tom")]
        public string GetData(int input)
        {
            return (input + 1).ToString();
        }
    }
}
