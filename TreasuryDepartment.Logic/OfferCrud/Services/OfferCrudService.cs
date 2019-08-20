using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreasureDepartment.Data;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Data.Enums;
using TreasureDepartment.Logic.OfferCrud.Models;

namespace TreasureDepartment.Logic.OfferCrud.Services
{
    public class OfferCrudService<TClassname>
        where TClassname : UsersOfferDbo
    {
        protected readonly TreasuryDepartmentContext _context;
        private readonly DbSet<TClassname> _dbSet;

        protected OfferCrudService()
        {
        }

        public OfferCrudService(TreasuryDepartmentContext context)
        {
            _context = context;
            _dbSet = _context.Set<TClassname>();
        }

        public async Task<TClassname> Get(UsersOfferRequest usersOfferRequest) =>
            await _dbSet
                .Include(classname => classname.SenderUser)
                .Include(classname => classname.TargetUser)
                .SingleOrDefaultAsync(item =>
                    item.SenderUserId == usersOfferRequest.SenderUserId &&
                    item.TargetUserId == usersOfferRequest.TargetUserId);

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

        public async Task<TClassname> Create(TClassname offer)
        {
            _dbSet.Add(offer);
            await _context.SaveChangesAsync();
            return offer;
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