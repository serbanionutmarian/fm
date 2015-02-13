using LightInject;
using SSService.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSService
{
    class LightInjectHelper
    {
        public static LightInjectContainerAdapter Init(IServiceContainer serviceContainer)
        {
            return Adapter = new LightInjectContainerAdapter(serviceContainer);
        }

        public static LightInjectContainerAdapter Adapter
        {
            get;
            private set;
        }
    }
}