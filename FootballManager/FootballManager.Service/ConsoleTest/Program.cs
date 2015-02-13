using System;
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
using System.ServiceModel.Channels;
using System.Net;
using ConsoleTest.Ioc.Modules;
using ServiceStack.ServiceClient.Web;
using Dto;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer<DbManagerContext>(null);
            RegisterUsingIoc((sc) =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Monitor(() => RunMethod(sc));
                }
            });
        }

        private static void RunMethod(ServiceContainer container)
        {
            ServiceCredentials.Instance.Init("test", "123");
            var service = new ServiceCaller.Services.AuthService();
            service.SignUp(new SignupRequest()
            {
                DisplayName = "a",
                TeamId = 2281
            });
        }

        private static void Monitor(Action action)
        {
            var start = DateTime.Now;
            action();
            Console.WriteLine(DateTime.Now - start);
        }

        private static void RegisterUsingIoc(Action<ServiceContainer> action)
        {
            using (var container = new ServiceContainer())
            {
                container.RegisterFrom<DataServiceModule>();
                container.RegisterFrom<RepositoryModule>();
                container.RegisterFrom<EntityFrameworkModule>();
                using (container.BeginScope())
                {
                    action(container);
                }
            }
        }
    }
}
