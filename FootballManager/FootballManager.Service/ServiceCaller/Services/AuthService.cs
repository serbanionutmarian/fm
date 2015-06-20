using Dto;
using Dto.Auth.Request;
using Dto.Auth.Response;
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
            return _caller.Run(client =>
             {
                 return client.Get<SignupResponse>(request);
             });


            ////SignupResponse response = null;
            //_caller.RunWithAuthentication(client =>
            //{
            //    return client.Post<SignupResponse>(request);
            //});
            return null;
        }
    }
}
