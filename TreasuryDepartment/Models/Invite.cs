using TreasuryDepartment.Models;

namespace TreasuryDepartment.Models
{
	public class Invite
	{
		public long SenderUserId { get; set; }
		public long TargetUserId { get; set; }
		public InviteStatus Status { get; set; } = InviteStatus.Created;

		public Invite(long senderUserId, long targetUserId)
		{
			SenderUserId = senderUserId;
			TargetUserId = targetUserId;
		}
	}

	public enum InviteStatus
	{
		Created,
		Accepted,
		Declined
	}
}