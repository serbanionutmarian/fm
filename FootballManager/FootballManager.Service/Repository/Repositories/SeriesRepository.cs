using DataModel.Tables;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class SeriesRepository : GenericRepository<Series>, ISeriesRepository
    {
        public SeriesRepository(DbContext context)
            : base(context)
        {

        }

        public IEnumerable<Series> GetByLeageIdAnCountryid(int leageConfigurationId, int countryId)
        {
            return FindBy(series => series.LeagesConfigurationId == leageConfigurationId && series.CountryId == countryId);
        }
    }
}
