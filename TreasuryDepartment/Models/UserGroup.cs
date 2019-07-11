using Microsoft.EntityFrameworkCore;

namespace TreasuryDepartment.Models
{
	public class UserGroup
	{
		public long UserId { get; set; }
		public User User { get; set; }

		public long GroupId { get; set; }
		public Group Group { get; set; }
	}
}