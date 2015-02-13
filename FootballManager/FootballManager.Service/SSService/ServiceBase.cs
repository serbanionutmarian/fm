using ServiceStack.ServiceInterface;
using SSService.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSService
{
    public class ServiceBase : Service
    {
        public CustomAuthUserSession CurrentSession
        {
            get
            {
                return this.GetSession(false) as CustomAuthUserSession;
            }
        }
    }
}