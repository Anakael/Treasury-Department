using System.Collections.Generic;

namespace TreasuryDepartment.Models
{
	public class Group
	{
		public long Id;
		public long Name;
		List<User> Users { get; set; }
	}
}