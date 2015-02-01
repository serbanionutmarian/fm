﻿using DataModel;
using LightInject;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SSService.Ioc.Modules
{
    public class EntityFrameworkModule : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<DbContext, DbManagerContext>(new PerRequestLifeTime());
            serviceRegistry.Register<IUnitOfWork, UnitOfWork>(new PerRequestLifeTime());
        }
    }
}