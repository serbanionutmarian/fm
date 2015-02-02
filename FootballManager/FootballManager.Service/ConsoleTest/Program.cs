﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataService;
using System.ComponentModel;
using DataService.Services;
using DataService.Interfaces;
using System.Data.Entity;
using DataModel;
using ServiceStack;
using System.ServiceModel;
using LightInject;
using Dto.Auth.Request;
using Dto.Auth.Response;
using System.Reflection;
using DataModel.Player;
using Autofac;
using WcfService.AutofacModules;

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
            //var service = scope.Resolve<IAuthService>();
            //service.SignUp(new SignupDto() { 
            //    DisplayName="Ionut S.",
            //    Email="serban.ionut.marian@gmail.com",
            //    Password="test..."
            //});

            //var service = scope.Resolve<IDataGeneratorService>();
            //service.AddLeagesToAllCountries();

            var service = scope.Resolve<IAuthService>();
            service.SignUp(new SignupRequest() { 
            });
            ///2288
        }

        private static void RunUsingIoc(Action<ILifetimeScope> action)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new DataServiceModule());
            builder.RegisterModule(new EntityFrameworkModule());

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                action(scope);
            }
        }
    }
}
