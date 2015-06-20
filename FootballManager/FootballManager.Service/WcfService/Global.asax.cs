using Autofac;
using Autofac.Integration.Wcf;
using DataService;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using WcfService.AutofacModules;
using System.Data.Entity;
using DataModel;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net;
using WcfService.Config;

namespace WcfService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Web.ApplicationServices.AuthenticationService.Authenticating += new EventHandler<System.Web.ApplicationServices.AuthenticatingEventArgs>(AuthenticationService_Authenticating);
            Database.SetInitializer<DbManagerContext>(null);
            InitIoc();
        }

        private void AuthenticationService_Authenticating(object sender, System.Web.ApplicationServices.AuthenticatingEventArgs e)
        {
            e.Authenticated = e.UserName == "ionut" && e.Password == "123";
            e.AuthenticationIsComplete = true;

            if (e.Authenticated)
            {
                new AuthenticationCookie().SetTicket(e.UserName);
            }
        }

        private static void InitIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new DataServiceModule());
            builder.RegisterModule(new EntityFrameworkModule());
            builder.RegisterModule(new WcfServiceModule());

            // build container
            AutofacHostFactory.Container = builder.Build();
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}