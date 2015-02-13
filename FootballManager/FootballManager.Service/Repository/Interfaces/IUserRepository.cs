using DataModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        User GetById(int userId);

        User GetBy(string email, string password);
    }
}
