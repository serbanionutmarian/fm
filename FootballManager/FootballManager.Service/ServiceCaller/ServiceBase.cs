using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCaller
{
    public abstract class ServiceBase
    {
        protected ServiceCaller _caller = ServiceCaller.Instance;
    }
}
