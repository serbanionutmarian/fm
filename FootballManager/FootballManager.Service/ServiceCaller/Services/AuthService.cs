using Dto;
using Dto.Request;
using Dto.Response;
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
                 var response = client.Post<SignupResponse>(request);
                 new ServiceCaller.Files.CredentialsFile().Save(new Files.CredentialsModel()
                 {
                     UserName = request.Email,
                     Password = request.Password
                 });
                 return response;
             });


            ////SignupResponse response = null;
            //_caller.RunWithAuthentication(client =>
            //{
            //    return client.Post<SignupResponse>(request);
            //});
        }
    }
}
