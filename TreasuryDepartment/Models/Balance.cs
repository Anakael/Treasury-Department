using Microsoft.EntityFrameworkCore;

namespace TreasuryDepartment.Models
{
	public class Balance
	{
		public long SourceUserId { get; set; }
		public User SourceUser { get; set; }
		public long TargetUserId { get; set; }
		public User TargetUser { get; set; }
		public decimal Sum { get; set; }
	}
}