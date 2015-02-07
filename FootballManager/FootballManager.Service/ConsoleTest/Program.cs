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

namespace ConsoleTest
{
    class Program
    {
        private static string cookie = null;

        static void Main(string[] args)
        {
            //Login();
            //Ping();

            Database.SetInitializer<DbManagerContext>(null);
            RegisterUsingIoc(RunMethod);
        }

        private static void RunMethod(ServiceContainer container)
        {
            var service = container.GetInstance<IAuthService>();
            var response = service.SignUp(new SignupRequest()
               {
               });
        }
        //static void Login()
        //{
        //    TestServices.AuthenticationServiceClient client = new TestServices.AuthenticationServiceClient();
        //    using (new OperationContextScope(client.InnerChannel))
        //    {
        //        bool result = client.Login("ionut", "123", string.Empty, false);
        //        var responseMessageProperty = (HttpResponseMessageProperty)
        //             OperationContext.Current.IncomingMessageProperties[HttpResponseMessageProperty.Name];

        //        if (result)
        //        {
        //            cookie = responseMessageProperty.Headers.Get("Set-Cookie");
        //        }
        //    }
        //}

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

        //static void Ping()
        //{
        //    ServiceReference1dd.Service1Client client = new ServiceReference1dd.Service1Client();

        //    using (new OperationContextScope(client.InnerChannel))
        //    {

        //        HttpRequestMessageProperty request = new HttpRequestMessageProperty();
        //        request.Headers[HttpResponseHeader.SetCookie] = cookie;
        //        OperationContext.Current.OutgoingMessageProperties
        //                 [HttpRequestMessageProperty.Name] = request;

        //        var result = client.DoWork(10);
        //    }
        //}
    }
}
