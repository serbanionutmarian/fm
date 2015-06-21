using DataService.Interfaces;
using Dto.Request;
using Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSService.Services
{
    public class TestService : ServiceBase
    {
        private readonly ICountryService _service;

        public TestService(ICountryService service)
        {
            _service = service;
        }
        public TestResponse Get(TestRequest request)
        {
            try
            {
                _service.GetAll();
                return new TestResponse()
                {
                    Result = "success"
                };
            }
            catch (Exception ex)
            {
                return new TestResponse()
                {
                    Result = "error: " + ex.Message
                };
            }


        }
    }
}