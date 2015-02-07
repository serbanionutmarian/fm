using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Ioc.Modules
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
