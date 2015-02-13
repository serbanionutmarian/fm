using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SSService.Config
{
    [DataContract]
    public class CustomAuthUserSession : AuthUserSession
    {
        [DataMember]
        public int? TeamId { get; set; }

    }
}