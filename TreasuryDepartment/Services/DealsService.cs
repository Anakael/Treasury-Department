using System.Threading.Tasks;
using TreasuryDepartment.Models;
using TreasuryDepartment.Services.OfferService;

namespace TreasuryDepartment.Services
{
    public class DealsService : OfferCrudWithUpdateService<Deal>
    {
        private readonly OfferCrudService<Balance> _balanceService;

        public DealsService(TreasuryDepartmentContext context) : base(context)
        {
            _balanceService = new OfferCrudService<Balance>(_context);
        }

        public new async Task Accept(Deal deal)
        {
            await base.Accept(deal);
            var senderBalance = new Balance(deal, -deal.Sum);
            senderBalance = await _balanceService.Get(senderBalance) ??
                            await _balanceService.Create(senderBalance);
            var reverseOffer = new Offer(deal);
            var tmpId = reverseOffer.TargetUserId;
            reverseOffer.TargetUserId = reverseOffer.SenderUserId;
            reverseOffer.SenderUserId = tmpId;
            var targetBalance = new Balance(reverseOffer, deal.Sum);
            targetBalance = await _balanceService.Get(targetBalance) ??
                            await _balanceService.Create(targetBalance);
        }
    }
}