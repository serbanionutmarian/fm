using DataModel.Tables;
using DataService.Interfaces;
using Ioc;
using LightInject;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using System.Globalization;

namespace SSService.Config
{
    public class AppHost : AppHostBase
    {
        //Tell ServiceStack the name of your application and where to find your services
        public AppHost()
            : base("Web Service", typeof(AppHost).Assembly)
        {

        }

        public override void Configure(Funq.Container container)
        {
            var serviceContainer = RegisterIoc(container);
            
            Plugins.Add(new AuthFeature(() => new CustomAuthUserSession(),
                new IAuthProvider[] { 
                    new EFCredentialsAuthProvider()
                }));

            Plugins.Add(new RegistrationFeature());
            container.Register<ICacheClient>(new MemoryCacheClient());
            container.Register<IUserAuthRepository>(new InMemoryAuthRepository());
        }

        private ServiceContainer RegisterIoc(Funq.Container container)
        {
            container.Adapter = LightInjectContainer.Adapter;

            var lightInjectContainer = LightInjectContainer.Container;
            lightInjectContainer.Register<IAuthProvider, EFCredentialsAuthProvider>(new PerScopeLifetime());
            lightInjectContainer.EnablePerWebRequestScope();

            return lightInjectContainer;
        }
    }
}