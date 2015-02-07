using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using LightInject;

namespace WcfService.AutofacModules
{
    public class RepositoryModule : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterAssembly(Assembly.Load("Repository"),
                () => new PerScopeLifetime(),
                (serviceType, implementingType) => serviceType.Name.EndsWith("Repository"));
        }
    }
}