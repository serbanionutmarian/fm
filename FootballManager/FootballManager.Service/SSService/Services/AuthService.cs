using DataService.Interfaces;
using Dto.Auth.Request;
using Dto.Auth.Response;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSService.Services
{
    public class AuthService : Service
    {
        private readonly IAuthService _service;

        public AuthService(IAuthService service)
        {
            _service = service;
        }

        public SignupResponse Post(SignupRequest request)
        {
            return _service.SignUp(request);
        }
    }
}