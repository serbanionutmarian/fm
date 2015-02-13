using DataModel.Tables;
using DataService.Interfaces;
using LightInject;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using SSService.Ioc;
using SSService.Ioc.Modules;
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
            var serviceContainer = new ServiceContainer();

            container.Adapter = LightInjectHelper.Init(serviceContainer);

            serviceContainer.Register<IAuthProvider, EFCredentialsAuthProvider>(new PerScopeLifetime());
            serviceContainer.RegisterFrom<DataServiceModule>();
            serviceContainer.RegisterFrom<RepositoryModule>();
            serviceContainer.RegisterFrom<EntityFrameworkModule>();
            serviceContainer.EnablePerWebRequestScope();

            return serviceContainer;
        }
    }
}