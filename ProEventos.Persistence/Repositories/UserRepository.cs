using ProEventos.Domain.Identity;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    public class UserRepository : ProEventosRepository, IUserRepository
    {
        private readonly ProEventosContext _context;
        public UserRepository(ProEventosContext context) : base(context)
        {
            _context = context;
        }
        public Task<User> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
