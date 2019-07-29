using System;

namespace TreasuryDepartment.Models
{
	public enum DealStatus
	{
		Pending,
		Accepted,
		Declined
	}

	public class Deal
	{
		public long SenderUserId { get; set; }
		public User SenderUser { get; set; }
		public long TargetUserId { get; set; }
		public User TargetUser { get; set; }
		public decimal Sum { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastStatusChangeDate { get; set; }
		public DealStatus Status { get; set; } = DealStatus.Pending;
	}
}