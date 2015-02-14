using LightInject;
using ServiceStack.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ioc
{
    public class LightInjectAdapter : IContainerAdapter
    {
        private readonly IServiceContainer _container;

        public LightInjectAdapter(IServiceContainer container)
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