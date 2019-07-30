using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<Friend> Get(long user1Id, long user2Id) =>
            await _context.Friends.FindAsync(new Friend(user1Id, user2Id));

        public async Task<Friend> Create(Friend friend)
        {
            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();
            return friend;
        }

        public async Task Delete(Friend friend)
        {
            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetFriends(long userId) =>
            await (
                from f in _context.Friends
                where f.User1Id == userId || f.User2Id == userId
                select f.User1Id == userId ? f.User2 : f.User1
            ).ToListAsync();
    }
}