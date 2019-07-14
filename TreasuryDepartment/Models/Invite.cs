using TreasuryDepartment.Models;

namespace TreasuryDepartment.Models
{
	public class Invite
	{
		public long SenderUserId { get; set; }
		public User SenderUser { get; set; }
		public long TargetUserId { get; set; }
		public User TargetUser { get; set; }
		public InviteStatus Status { get; set; } = InviteStatus.Created;
	}

	public enum InviteStatus
	{
		Created,
		Accepted,
		Declined
	}
}