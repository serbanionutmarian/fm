using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Repository;
using DataModel;
using LightInject;

namespace WcfService.AutofacModules
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