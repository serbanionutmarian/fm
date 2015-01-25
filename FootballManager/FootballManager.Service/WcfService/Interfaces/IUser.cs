using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WcfService.Interfaces
{
    [ServiceContract]
    public interface IUser
    {
        [OperationContract]
        User GetByUserId(int userId);
    }
}