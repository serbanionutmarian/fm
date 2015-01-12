using DtoModel.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService.Interfaces
{
    [ServiceContract]
    public interface IAuthService
    {
        [OperationContract]
        void SignUp(SignupDto input);
    }
}
