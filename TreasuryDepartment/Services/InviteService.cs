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
		private readonly FriendService _friendService;

		public InviteService(TreasuryDepartmentContext context, FriendService friendService)
		{
			_context = context;
			_friendService = friendService;
		}

		public async Task<Invite> Get(long senderUserId, long targetUserId) =>
			await _context.Invites.FindAsync(senderUserId, targetUserId);

		public async Task<List<Invite>> GetReceivedInvites(long targetUserId) =>
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

		private async Task ChangeStatus(Invite invite, InviteStatus newStatus)
		{
			invite.Status = newStatus;
			_context.Entry(invite).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task<Invite> Create(Invite invite)
		{
			_context.Invites.Add(invite);
			await _context.SaveChangesAsync();
			return invite;
		}

		public async Task<Friend> Accept(Invite invite)
		{
			await ChangeStatus(invite, InviteStatus.Accepted);
			return await _friendService.Create(new Friend(invite.SenderUserId, invite.TargetUserId));
		}

		public async Task Decline(Invite invite) =>
			await ChangeStatus(invite, InviteStatus.Declined);


		public async Task Delete(Invite invite)
		{
			_context.Remove(invite);
			await _context.SaveChangesAsync();
		}
	}
}