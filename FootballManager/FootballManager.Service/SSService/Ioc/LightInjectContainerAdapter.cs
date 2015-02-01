using LightInject;
using ServiceStack.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSService.Ioc
{
    public class LightInjectContainerAdapter : IContainerAdapter
    {
        private readonly IServiceContainer _container;

        public LightInjectContainerAdapter(IServiceContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.GetInstance<T>();
        }

        public T TryResolve<T>()
        {
            return _container.TryGetInstance<T>();
        }
    }
}