using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Ioc.Modules
{
    public class DataServiceModule : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterAssembly(Assembly.Load("DataService"),
                () => new PerScopeLifetime(),
                (serviceType, implementingType) => serviceType.Name.EndsWith("Service"));
        }
    }
}
