using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using DataService;
using DataService.Interfaces;

namespace WcfService.Modules
{
    public class DataServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("DataService"))
                      .Where(t => t.Name.EndsWith("Service"))
                      .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        }
    }
}