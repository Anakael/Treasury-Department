using System.Threading.Tasks;
using TreasuryDepartment.Models;
using TreasuryDepartment.Models.RequestModels;
using TreasuryDepartment.Services.OfferService;

namespace TreasuryDepartment.Services
{
    public class DealsService
    {
        private readonly TreasuryDepartmentContext _context;
        private readonly OfferCrudService<Deal> _dealsService;

        public DealsService(TreasuryDepartmentContext context, OfferCrudService<Deal> dealsService)
        {
            _context = context;
            _dealsService = dealsService;
        }

        private async Task<Balance> Get(RequestUsersOffer requestUsersOffer) =>
            await _context.Balances.FindAsync(requestUsersOffer.SenderUserId, requestUsersOffer.TargetUserId);

        private async Task<Balance> Create(Balance balance)
        {
            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();
            return balance;
        }

        public async Task Accept(Deal deal)
        {
            await _dealsService.Accept(deal);
            var senderBalance = new Balance(deal, -deal.Sum);
            senderBalance = await Get(senderBalance) ??
                            await Create(senderBalance);
            var reverseOffer = new Offer(deal);
            var tmpId = reverseOffer.TargetUserId;
            reverseOffer.TargetUserId = reverseOffer.SenderUserId;
            reverseOffer.SenderUserId = tmpId;
            var targetBalance = new Balance(reverseOffer, deal.Sum);
            targetBalance = await Get(targetBalance) ??
                            await Create(targetBalance);
        }
    }
}