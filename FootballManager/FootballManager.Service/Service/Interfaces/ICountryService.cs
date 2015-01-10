using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Interfaces
{
    public interface ICountryService : IEntityService<Country>
    {
        Country GetById(int Id);
    }
}
