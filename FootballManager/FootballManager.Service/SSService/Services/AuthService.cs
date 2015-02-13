using DataService.Interfaces;
using Dto.Auth.Request;
using Dto.Auth.Response;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using SSService.Config;

namespace SSService.Services
{
    public class AuthService : ServiceBase
    {
        private readonly IAuthService _service;

        public AuthService(IAuthService service)
        {
            _service = service;
        }
        [OwnTeam]
        public SignupResponse Post(SignupRequest request)
        {
            return new SignupResponse()
            {
                Result = CurrentSession.TeamId.Value.ToString()
            };
        }
    }
}