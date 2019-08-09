using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreasuryDepartment.Models;
using TreasuryDepartment.Models.Enums;

namespace TreasuryDepartment.Services.OfferService
{
    public class OfferCrudWithUpdateService<TClassname> : OfferCrudService<TClassname>
        where TClassname : Offer
    {
        protected OfferCrudWithUpdateService()
        {
        }

        public OfferCrudWithUpdateService(TreasuryDepartmentContext context) : base(context)
        {
        }

        private async Task ChangeStatus(TClassname offer, Status newStatus)
        {
            offer.Status = newStatus;
            _context.Entry(offer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Accept(TClassname offer) =>
            await ChangeStatus(offer, Status.Accepted);


        public async Task Decline(TClassname offer) =>
            await ChangeStatus(offer, Status.Declined);


        public async Task Delete(TClassname offer)
        {
            _dbSet.Remove(offer);
            await _context.SaveChangesAsync();
        }
    }
}