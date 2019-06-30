using System.Collections.Generic;

namespace TreasuryDepartment.Models
{
	public class User
	{
		public long Id { get; set; }
		public string Name { get; set; }
		List<Group> Groups { get; set; }
	}
}