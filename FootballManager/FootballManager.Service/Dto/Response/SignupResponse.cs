using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Dto.Auth.Response
{
    [DataContract]
    public class SignupResponse
    {
        [DataMember]
        public string Result { get; set; }
    }
}
