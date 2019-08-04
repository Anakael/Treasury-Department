using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreasuryDepartment.Models;

namespace TreasuryDepartment.Services
{
    public class FriendService
    {
        private readonly TreasuryDepartmentContext _context;

        public FriendService(TreasuryDepartmentContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetFriends(long userId) =>
            await (
                from f in _context.Friends
                where f.SenderUserId == userId || f.TargetUserId == userId
                select f.SenderUserId == userId ? f.TargetUser : f.SenderUser
            ).ToListAsync();
    }
}