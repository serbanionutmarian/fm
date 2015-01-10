using Model.Tables;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class LeagesConfigurationRepository : GenericRepository<LeagesConfiguration>, ILeagesConfigurationRepository
    {
        public LeagesConfigurationRepository(DbContext context)
            : base(context)
        {

        }
    }
}
