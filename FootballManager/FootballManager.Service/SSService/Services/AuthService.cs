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
        private readonly IDataGeneratorService _service;

        public AuthService(IDataGeneratorService service)
        {
            _service = service;
        }

        public SignupResponse Get(SignupRequest request)
        {
            _service.AddLeagesToAllCountries();
            //  _service.SignUp(request);

            return new SignupResponse()
            {
                Result = request.ToString()
            };
        }
    }
}