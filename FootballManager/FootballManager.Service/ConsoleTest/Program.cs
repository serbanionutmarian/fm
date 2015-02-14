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
using ServiceStack.ServiceClient.Web;
using Dto;
using Ioc;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer<DbManagerContext>(null);
            RegisterUsingIoc(RunMethod);
        }

        private static void RunMethod(ServiceContainer container)
        {
            var gg = container.GetInstance<DataService.Jobs.Items.TacticJobExecutionService>();
            gg.Execute();            

            //ServiceCredentials.Instance.Init("test", "123");
            //var service = new ServiceCaller.Services.AuthService();
            //service.SignUp(new SignupRequest()
            //{
            //    DisplayName = "a",
            //    TeamId = 2281
            //});
        }

        private static void Monitor(Action action)
        {
            var start = DateTime.Now;
            action();
            Console.WriteLine(DateTime.Now - start);
        }

        private static void RegisterUsingIoc(Action<ServiceContainer> action)
        {
            using (var container = LightInjectContainer.Container)
            using (container.BeginScope())
            {
                action(container);
            }
        }
    }
}
