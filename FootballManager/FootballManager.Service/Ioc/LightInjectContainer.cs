using Ioc.Modules;
using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ioc
{
    public class LightInjectContainer
    {
        static LightInjectContainer()
        {
            Container = new ServiceContainer();
            Adapter = new LightInjectAdapter(Container);

            Container.Register<IServiceContainer>((factory) => Container, new PerScopeLifetime());
            Container.RegisterFrom<DataServiceModule>();
            Container.RegisterFrom<RepositoryModule>();
            Container.RegisterFrom<EntityFrameworkModule>();
        }

        public static LightInjectAdapter Adapter
        {
            get;
            private set;
        }

        public static ServiceContainer Container
        {
            get;
            private set;
        }
    }
}