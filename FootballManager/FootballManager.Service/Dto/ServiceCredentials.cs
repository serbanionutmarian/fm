using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dto
{
    /// <summary>
    /// must be initilized when the application starts
    /// </summary>
    public class ServiceCredentials
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        private static ServiceCredentials _instance;
        public static ServiceCredentials Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceCredentials();
                }
                return _instance;
            }
        }
        public void Init(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        private ServiceCredentials()
        {
        }
    }
}
