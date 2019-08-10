using System.Threading.Tasks;
using TreasuryDepartment.Models;

namespace TreasuryDepartment.Services
{
    public class UserService
    {
        private readonly TreasuryDepartmentContext _context;

        public UserService(TreasuryDepartmentContext context)
        {
            _context = context;
        }

        public async Task<User> Get(long id) =>
            await _context.Users.FindAsync(id);

        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}