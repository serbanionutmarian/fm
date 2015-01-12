using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Auth
{
    [DataContract]
    public class SignupDto
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
