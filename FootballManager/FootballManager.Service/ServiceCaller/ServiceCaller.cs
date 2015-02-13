using Dto;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCaller
{
    public class ServiceCaller : IDisposable
    {
        private const string ServiceUrl = "http://localhost:62333/";

        private static ServiceCaller _instance;
        public static ServiceCaller Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceCaller();
                }
                return _instance;
            }
        }
        private ServiceCredentials _credentials;
        private bool _isAuthenticated = false;

        private JsonServiceClient _client;
        private ServiceCaller()
        {
            _credentials = ServiceCredentials.Instance;
            _client = new JsonServiceClient(ServiceUrl);
            _client.AlwaysSendBasicAuthHeader = true;
        }
        public void RunWithAuthentication(Action<JsonServiceClient> action)
        {
            EnsureAuthentication();
            try
            {
                action(_client);
            }
            catch (WebServiceException ex)
            {
                // subsequent requests for unauthorized(may be the server has been resarted)
                if (ex.StatusCode == 401)
                {
                    Authenticate();
                    action(_client);
                }
            }
        }

        private void EnsureAuthentication()
        {
            if (!_isAuthenticated)
            {
                _isAuthenticated = true;
                Authenticate();
            }
        }

        private void Authenticate()
        {
            _client.Get<ServiceStack.Common.ServiceClient.Web.AuthResponse>(new ServiceStack.Common.ServiceClient.Web.Auth()
            {
                UserName = _credentials.UserName,
                Password = _credentials.Password,
                RememberMe = true
            });
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
