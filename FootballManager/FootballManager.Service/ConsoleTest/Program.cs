using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Web.Mvc;
using DataService;
using System.ComponentModel;
using DataService.Services;
using DataService.Interfaces;
using AutofacModules.Modules;
using System.Data.Entity;
using DataModel;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer<DbManagerContext>(null);
            RunUsingIoc(RunMethod);
        }
        private static void RunMethod(ILifetimeScope scope)
        {
            var service = scope.Resolve<IDataGeneratorService>();
            service.AddLeagesToAllCountries();
        }

        private static void RunUsingIoc(Action<ILifetimeScope> action)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                action(scope);
            }
        }
    }
}
