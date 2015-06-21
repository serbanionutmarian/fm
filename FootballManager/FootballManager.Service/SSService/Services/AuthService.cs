using DataService.Interfaces;
using Dto.Request;
using Dto.Response;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using SSService.Config;
using System;

namespace SSService.Services
{
    public class AuthService : ServiceBase
    {
        private readonly IAuthService _service;

        public AuthService(IAuthService service)
        {
            _service = service;
        }
        //[OwnTeam]
        public SignupResponse Get(SignupRequest request)
        {
            return new SignupResponse()
            {
                Result = "3"// CurrentSession.TeamId.Value.ToString()
            };
        }
    }
}