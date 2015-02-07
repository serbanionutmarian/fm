using LightInject;
using ServiceStack.WebHost.Endpoints;
using SSService.Ioc;
using SSService.Ioc.Modules;

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
            RegisterIoc(container);
        }

        private void RegisterIoc(Funq.Container container)
        {
            var serviceContainer = new ServiceContainer();

            container.Adapter = new LightInjectContainerAdapter(serviceContainer);

            serviceContainer.RegisterFrom<DataServiceModule>();
            serviceContainer.RegisterFrom<RepositoryModule>();
            serviceContainer.RegisterFrom<EntityFrameworkModule>();
            serviceContainer.EnablePerWebRequestScope();
        }
    }
}