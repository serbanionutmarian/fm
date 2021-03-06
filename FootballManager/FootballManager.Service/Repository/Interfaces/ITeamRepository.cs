﻿using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Team GetRandomBoot(int countryId);

        IEnumerable<Team> GetAllBySeriesId(int seriesId);
    }
}
