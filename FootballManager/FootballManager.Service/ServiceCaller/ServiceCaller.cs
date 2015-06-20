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
        public T GetResponseWithError<T>(bool authenticatedError)
            where T : ResponseBase, new()
        {
            var response = ResponseBase.CreateValidationError<T>("Unexpected error");
            response.Error.IsValidationError = false;
            response.Error.AuthenticatedError = authenticatedError;
            return response;
        }
        public T Run<T>(Func<JsonServiceClient, T> action)
            where T : ResponseBase, new()
        {
            try
            {
                return action(_client);
            }
            catch (ServiceStack.ServiceClient.Web.WebServiceException)
            {
                return GetResponseWithError<T>(false);
            }
        }
        public T RunWithAuthentication<T>(Func<JsonServiceClient, T> action)
            where T : ResponseBase, new()
        {
            EnsureAuthentication();
            try
            {
                return action(_client);
            }
            catch (WebServiceException ex1)
            {
                // subsequent requests for unauthorized(may be the server has been resarted)
                if (ex1.StatusCode == 401)
                {
                    Authenticate();
                    try
                    {
                        return action(_client);
                    }
                    catch (WebServiceException ex2)
                    {
                        if (ex2.StatusCode == 401)
                        {
                            return GetResponseWithError<T>(true);
                        }
                    }
                }
                return GetResponseWithError<T>(false);
            }
        }

        private void EnsureAuthentication()
        {
            if (!_isAuthenticated)
            {
                Authenticate();
            }
        }

        private void Authenticate()
        {
            var response = _client.Get<ServiceStack.Common.ServiceClient.Web.AuthResponse>(new ServiceStack.Common.ServiceClient.Web.Auth()
               {
                   UserName = _credentials.UserName,
                   Password = _credentials.Password,
                   RememberMe = true
               });
            _isAuthenticated = true;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
