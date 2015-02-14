using DataModel.Tables;
using DataService.Interfaces;
using Ioc;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SSService.Config
{
    public class EFCredentialsAuthProvider : CredentialsAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            var authEF = LightInjectContainer.Adapter.Resolve<IAuthService>();
            CustomAuthUserSession to = authService.GetSession(false) as CustomAuthUserSession;
            User user = null;
            if (!authEF.TryAuthenticate(userName, password, out user))
            {
                return false;
            }
            to.TeamId = user.TeamId;
            to.IsAuthenticated = true;
            to.UserAuthId = user.Id.ToString(CultureInfo.InvariantCulture);
            return true;
        }
    }
}