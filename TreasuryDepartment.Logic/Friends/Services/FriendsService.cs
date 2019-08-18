using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreasureDepartment.Data;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Data.Enums;
using TreasureDepartment.Logic.OfferCrud.Services;

namespace TreasureDepartment.Logic.Friends.Services
{
    public class FriendService : OfferCrudService<FriendInviteDbo>
    {
        public FriendService(TreasuryDepartmentContext context) : base(context)
        {
        }

        public async Task<List<UserDbo>> GetFriends(long userId) =>
            await (
                from f in Context.Friends
                where (f.SenderUserId == userId || f.TargetUserId == userId) && f.Status == Status.Accepted
                select f.SenderUserId == userId ? f.TargetUser : f.SenderUser
            ).ToListAsync();
    }
}