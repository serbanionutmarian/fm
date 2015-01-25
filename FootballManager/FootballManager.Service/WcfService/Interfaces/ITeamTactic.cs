using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Interfaces
{
    public interface ITeamTactic
    {
        DataModel.Tables.TeamTactic GetById(int teamId);
    }
}