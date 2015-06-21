using Dto.Request;
using Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCaller.Services
{
    public class TestService : ServiceBase
    {
        public TestResponse Test(TestRequest request)
        {
            return _caller.Run(client =>
            {
                return client.Get<TestResponse>(request);
            });
        }
    }
}
