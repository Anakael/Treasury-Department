using TreasuryDepartment.Models;

namespace TreasuryDepartment.Models
{
	public class Invite
	{
		public long GroupId { get; set; }
		public Group Group { get; set; }
		public long UserId { get; set; }
		public User User { get; set; }
	}
}