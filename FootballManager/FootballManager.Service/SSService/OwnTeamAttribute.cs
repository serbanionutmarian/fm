using Dto;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace SSService.Config
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OwnTeamAttribute : AuthenticateAttribute
    {
        public OwnTeamAttribute(params string[] roles)
            : this(ApplyTo.All, roles)
        {
        }

        public OwnTeamAttribute(ApplyTo applyTo, params string[] roles)
        {
            base.ApplyTo = applyTo;
            base.Priority = -90;
        }

        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            var dto = requestDto as ITeam;
            var session = req.GetSession(false) as CustomAuthUserSession;

            if (session.TeamId.HasValue && session.TeamId.Value != dto.TeamId)
            {
                res.StatusCode = 0x193;
                res.StatusDescription = "Invalid Role:" + dto.TeamId + "." + session.TeamId.Value;
                res.EndRequest(false);
            }
        }
    }
}