using Dto.Auth.Response;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Dto.Auth.Request
{
    [DataContract]
    public class SignupRequest
    {
        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public int CountryId { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}
