using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreasuryDepartment.Models;
using TreasuryDepartment.Services.OfferService;

namespace TreasuryDepartment.Services
{
    public class DealsService : OfferCrudService<Deal>
    {
        private readonly OfferCrudService<FriendInvite> _friendService;

        public DealsService(TreasuryDepartmentContext context) : base(context)
        {
            _friendService = new OfferCrudService<FriendInvite>(_context);
        }

        public new async Task Accept(Deal deal)
        {
            await base.Accept(deal);
            var friends = await _friendService.Get(new FriendInvite(deal.SenderUserId, deal.TargetUserId));
            friends.Sum -= deal.Sum;
            _context.Entry(friends).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}