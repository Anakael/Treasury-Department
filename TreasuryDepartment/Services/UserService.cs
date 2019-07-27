using TreasuryDepartment.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TreasuryDepartment.Services
{
	public class UserService
	{
		private readonly TreasuryDepartmentContext _context;

		public UserService(TreasuryDepartmentContext context)
		{
			_context = context;
		}

		public async Task<User> Get(long Id) =>
			await _context.Users.FindAsync(Id);

		public async Task Create(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
		}

	}
}