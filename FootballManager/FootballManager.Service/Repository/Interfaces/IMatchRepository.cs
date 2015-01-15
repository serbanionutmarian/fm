using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        void DeleteAll();
    }
}
