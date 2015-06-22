using Dto;
using Dto.Request;
using Dto.Response;
using ServiceCaller.Files;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCaller
{
    public class ServiceHelper : IDisposable
    {
        private const string ServiceUrl = "http://localhost:62333/";

        private static ServiceHelper _instance;
        public static ServiceHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceHelper();
                }
                return _instance;
            }
        }

        private CredentialsFile _credentialsFile;
        private CredentialsModel _credentials;
        private bool _isAuthenticated = false;

        private JsonServiceClient _client;
        private ServiceHelper()
        {
            _credentialsFile = new CredentialsFile();
            _credentials = _credentialsFile.Load();
            _client = new JsonServiceClient(ServiceUrl);
            _client.AlwaysSendBasicAuthHeader = true;
        }
        public T GetResponseWithUnexpectdError<T>(bool authenticatedError)
            where T : ResponseBase, new()
        {
            var response = ResponseBase.CreateUnexpectedError<T>();
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
                return GetResponseWithUnexpectdError<T>(false);
            }
        }
        public T RunWithAuthentication<T>(Func<JsonServiceClient, T> action)
            where T : ResponseBase, new()
        {
            bool prevIsAuthenticated = _isAuthenticated;
            if (!_isAuthenticated)
            {
                var responseError = TryToAuthenticate<T>();
                if (responseError != null)
                {
                    return responseError;
                }
                _isAuthenticated = true;
            }
            try
            {
                return action(_client);
            }
            catch (WebServiceException ex1)
            {
                // subsequent requests for unauthorized(may be the server has been resarted)
                if (ex1.StatusCode == 401)
                {
                    if (!prevIsAuthenticated)
                    {
                        return GetResponseWithUnexpectdError<T>(true);
                    }
                    var responseError = TryToAuthenticate<T>();
                    if (responseError != null)
                    {
                        return responseError;
                    }
                    try
                    {
                        return action(_client);
                    }
                    catch (WebServiceException ex2)
                    {
                        if (ex2.StatusCode == 401)
                        {
                            return GetResponseWithUnexpectdError<T>(true);
                        }
                    }
                }
                return GetResponseWithUnexpectdError<T>(false);
            }
        }

        private T TryToAuthenticate<T>()
             where T : ResponseBase, new()
        {
            if (_credentials == null)
            {
                return GetResponseWithUnexpectdError<T>(true);
            }
            int responseCode;
            try
            {
                var response = _client.Get<ServiceStack.Common.ServiceClient.Web.AuthResponse>(new ServiceStack.Common.ServiceClient.Web.Auth()
                   {
                       UserName = _credentials.UserName,
                       Password = _credentials.Password,
                       RememberMe = true
                   });
                responseCode = 200;
            }
            catch (WebServiceException ex)
            {
                responseCode = ex.StatusCode;
            }
            if (responseCode == 401)
            {
                return GetResponseWithUnexpectdError<T>(true);
            }
            else if (responseCode != 200)
            {
                return GetResponseWithUnexpectdError<T>(false);
            }
            return null;
        }

        public SignupResponse SignUp(SignupRequest request)
        {
            return Run(client =>
              {
                  var response = client.Post<SignupResponse>(request);
                  _credentials = new Files.CredentialsModel()
                  {
                      UserName = request.Email,
                      Password = request.Password
                  };
                  _credentialsFile.Save(_credentials);
                  return response;
              });
        }

        public bool IsLogIn()
        {
            return _credentialsFile.Exists();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
