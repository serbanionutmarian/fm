using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WcfService.AutofacModules
{
    public class WcfServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("WcfService"))
                      .Where(t => t.Name.EndsWith("Service")).AsSelf()
                //.AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        }
    }
}