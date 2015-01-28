using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WcfService.Interfaces
{
    [ServiceContract]
    public interface ITeamTacticService
    {
        [OperationContract]
        DataModel.Tables.TeamTactic GetById(int teamId);
    }
}