using DataModel;
using LightInject;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Ioc.Modules
{
    public class EntityFrameworkModule : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<DbContext, DbManagerContext>(new PerScopeLifetime());
            serviceRegistry.Register<IUnitOfWork, UnitOfWork>(new PerScopeLifetime());
        }
    }
}
