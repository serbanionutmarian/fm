using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Dto.Auth.Response
{
    public class SignupResponse : ResponseBase
    {
        public string Result { get; set; }
    }
}
