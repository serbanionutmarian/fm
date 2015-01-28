using DataModel;
using DataModel.Tables;
using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using WcfService.Interfaces;

namespace WcfService.Services
{
    public class UserService : WcfService.Interfaces.IUserService
    {
        private readonly DataService.Interfaces.IUserService _userService;

        public UserService(DataService.Interfaces.IUserService userService)
        {
            _userService = userService;
        }
        public DataModel.Tables.User GetByUserId(int userId)
        {
            throw new System.ServiceModel.Web.WebFaultException(System.Net.HttpStatusCode.NotModified);
            var etag = HttpContext.Current.Request.Headers["ETag"];
            // no modified
            HttpContext.Current.Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
            return _userService.GetById(userId);
        }
    }
}
