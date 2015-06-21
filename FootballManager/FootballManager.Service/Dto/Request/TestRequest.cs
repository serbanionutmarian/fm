using Dto.Response;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dto.Request
{
    [Route("/testService")]
    public class TestRequest : IReturn<TestResponse>
    {
        public string Request { get; set; }
    }
}
