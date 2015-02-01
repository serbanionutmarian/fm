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
using SSService.Ioc;
using SSService.Ioc.Modules;
using Dto.Auth.Request;
using Dto.Auth.Response;

namespace ConsoleTest
{
    class Program
    {

        static void Main(string[] args)
        {
            var restClient = new JsonServiceClient("http://localhost:62333/api");
            var res = restClient.Post<SignupResponse>(new SignupRequest()
                {
                    CountryId = 1,
                    DisplayName = "Ionut S.",
                    Email = "serban..",
                    Password = ".."
                });
            RegisterUsingIoc(container =>
            {
                //var service = container.GetInstance<Auth>();
                //var countries = service.GetAll();
            });
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

        public static void Monitor(string testName, Action action, int count = 1)
        {
            Console.WriteLine(testName);
            var start = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                var start1 = DateTime.Now;
                action();
                Console.WriteLine(string.Format("{2}.Test for {0}:{1}", testName, DateTime.Now - start1, i));
            }
            Console.WriteLine(string.Format("Test for {0}:{1}", testName, DateTime.Now - start));
        }
    }
}
