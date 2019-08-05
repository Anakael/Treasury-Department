using System.Threading.Tasks;
using TreasuryDepartment.Models;
using TreasuryDepartment.Models.Enums;

namespace TreasuryDepartment.Services.OfferService
{
    public class OfferService<TClassname>
        where TClassname : Offer
    {
        private readonly OfferCrudService<TClassname> _offerCrudService;

        public OfferService(OfferCrudService<TClassname> offerService)
        {
            _offerCrudService = offerService;
        }

        public async Task<Balance> Accept(Deal offer)
        {
            await _offerCrudService.Accept(offer as TClassname);
            var senderBalance = new Balance(offer);
            senderBalance = await _offerCrudService.Get(senderBalance) as Balance ??
                            await _offerCrudService.Create(senderBalance as TClassname) as Balance;
            senderBalance.Sum -= offer.Sum;
            Offer reverseOffer = offer;
            long tmpId = reverseOffer.TargetUserId;
            reverseOffer.TargetUserId = reverseOffer.SenderUserId;
            reverseOffer.SenderUserId = tmpId;
            var targetBalance = new Balance(offer);
            targetBalance = await _offerCrudService.Get(targetBalance) as Balance ??
                            await _offerCrudService.Create(targetBalance as TClassname) as Balance;
            targetBalance.Sum += offer.Sum;
            return senderBalance;
        }
    }
}