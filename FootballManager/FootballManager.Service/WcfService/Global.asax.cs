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

namespace WcfService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Database.SetInitializer<DbManagerContext>(null);
            InitIoc();
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