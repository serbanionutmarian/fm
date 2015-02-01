using Dto.Auth.Response;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Auth.Request
{
    [Route("/signup")]
    //  [Route("/signup", Verbs = "POST")]
    public class SignupRequest : IReturn<SignupResponse>
    {
        public string DisplayName { get; set; }

        public int CountryId { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
