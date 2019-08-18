using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreasureDepartment.Data;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Logic.OfferCrud.Models;
using TreasureDepartment.Logic.OfferCrud.Services;

namespace TreasureDepartment.Logic.Deals.Services
{
    public class DealsService : OfferCrudService<DealDbo>
    {
        private readonly OfferCrudService<FriendInviteDbo> _friendService;

        public DealsService(TreasuryDepartmentContext context) : base(context)
        {
            _friendService = new OfferCrudService<FriendInviteDbo>(Context);
        }

        public new async Task Accept(DealDbo dealDbo)
        {
            await base.Accept(dealDbo);
            var friends = await _friendService.Get(new UsersOfferRequest
            {
                SenderUserId = dealDbo.SenderUserId,
                TargetUserId = dealDbo.TargetUserId
            });
            friends.Sum -= dealDbo.Sum;
            Context.Entry(friends).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
    }
}