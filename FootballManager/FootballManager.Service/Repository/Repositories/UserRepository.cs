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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context)
            : base(context)
        {

        }

        public User GetById(int userId)
        {
            return _dbset.Include(user => user.Team).Single(user => user.Id == userId);
        }


        public User GetBy(string email, string password)
        {
            return _dbset.SingleOrDefault(user => user.Email == email && user.Password == password);
        }
    }
}
