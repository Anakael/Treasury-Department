using TreasuryDepartment.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TreasuryDepartment.Services
{
	public class InviteService
	{
		private readonly TreasuryDepartmentContext _context;

		public InviteService(TreasuryDepartmentContext context)
		{
			_context = context;
		}

		public async Task<List<Invite>> GetReciviedInvites(long targetUserId) =>
			await (
				from i in _context.Invites
				where i.TargetUserId == targetUserId
				select i
			).ToListAsync();

		public async Task<List<Invite>> GetSentInvites(long senderUserId) =>
			await (
				from i in _context.Invites
				where i.SenderUserId == senderUserId
				select i
			).ToListAsync();

		private IQueryable<Invite> Get(long senderUserId, long targetUserId) =>
			from i in _context.Invites.AsNoTracking()
			where i.SenderUserId == senderUserId && i.TargetUserId == targetUserId
			select i;

		private async Task ChangeStatus(long senderUserId, long targetUserId, InviteStatus newStatus)
		{
			Invite invite = await Get(senderUserId, targetUserId).FirstAsync();

			if (invite != null)
			{
				invite.Status = newStatus;
				_context.Entry(invite).State = EntityState.Modified;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<Invite> Create(Invite invite)
		{
			_context.Invites.Add(invite);
			await _context.SaveChangesAsync();
			return invite;
		}

		public async Task Accept(long senderUserId, long targetUserId) =>
			await ChangeStatus(senderUserId, targetUserId, InviteStatus.Accepted);

		public async Task Decline(long senderUserId, long targetUserId) =>
			await ChangeStatus(senderUserId, targetUserId, InviteStatus.Declined);

		public async Task Delete(long senderUserId, long targetUserId)
		{
			Invite invite = await Get(senderUserId, targetUserId).FirstAsync();

			if (invite != null && invite.Status != InviteStatus.Created)
			{
				_context.Remove(invite);
				await _context.SaveChangesAsync();
			}
		}
	}
}