using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreasuryDepartment.Models;
using TreasuryDepartment.Models.RequestModels;
using TreasuryDepartment.Models.Enums;

namespace TreasuryDepartment.Services.OfferService
{
    public class OfferCrudService<TClassname>
        where TClassname : Offer
    {
        private readonly TreasuryDepartmentContext _context;
        private readonly DbSet<TClassname> _dbSet;

        public OfferCrudService(TreasuryDepartmentContext context)
        {
            _context = context;
            _dbSet = _context.Set<TClassname>();
        }

        public async Task<TClassname> Get(RequestUsersOffer offer) =>
            await _dbSet.FindAsync(offer.SenderUserId, offer.TargetUserId);

        public async Task<List<TClassname>> GetReceivedOffers(long targetUserId) =>
            await (
                from i in _dbSet
                where i.TargetUserId == targetUserId
                select i
            ).Include(i => i.SenderUser).ToListAsync();

        public async Task<List<TClassname>> GetSentOffers(long senderUserId) =>
            await (
                from i in _dbSet
                where i.SenderUserId == senderUserId
                select i
            ).Include(i => i.TargetUser).ToListAsync();

        private async Task ChangeStatus(TClassname offer, Status newStatus)
        {
            offer.Status = newStatus;
            _context.Entry(offer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<TClassname> Create(TClassname offer)
        {
            _dbSet.Add(offer);
            await _context.SaveChangesAsync();
            return offer;
        }

        public async Task Accept(TClassname offer) =>
            await ChangeStatus(offer, Status.Accepted);

        public async Task<Balance> Accept(Deal offer)
        {
            await Accept(offer as TClassname);
            var senderBalance = new Balance(offer);
            senderBalance = await Get(senderBalance) as Balance ??
                            await Create(senderBalance as TClassname) as Balance;
            senderBalance.Sum -= offer.Sum;
            Offer reverseOffer = offer;
            long tmpId = reverseOffer.TargetUserId;
            reverseOffer.TargetUserId = reverseOffer.SenderUserId;
            reverseOffer.SenderUserId = tmpId;
            var targetBalance = new Balance(offer);
            targetBalance = await Get(targetBalance) as Balance ??
                            await Create(targetBalance as TClassname) as Balance;
            targetBalance.Sum += offer.Sum;
            return senderBalance;
        }

        public async Task Decline(TClassname offer) =>
            await ChangeStatus(offer, Status.Declined);


        public async Task Delete(TClassname offer)
        {
            _dbSet.Remove(offer);
            await _context.SaveChangesAsync();
        }
    }
}