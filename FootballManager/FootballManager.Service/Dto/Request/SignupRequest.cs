using Dto.Auth.Response;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Dto.Auth.Request
{
    [Authenticate]
    [Route("/signup")]
    [Route("/signup", Verbs = "POST")]
    public class SignupRequest : IReturn<SignupResponse>, ITeam
    {
        public string DisplayName { get; set; }

        public int CountryId { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int TeamId { get; set; }
    }
}
