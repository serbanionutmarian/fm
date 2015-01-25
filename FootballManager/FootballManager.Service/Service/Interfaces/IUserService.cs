using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Interfaces
{
    public interface IUserService : IEntityService<User>
    {
        User GetById(int userId);
    }
}
