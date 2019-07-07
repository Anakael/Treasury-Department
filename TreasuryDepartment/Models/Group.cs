using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TreasuryDepartment.Models
{
	public class Group
	{
		public long Id { get; set; }
		public long Name { get; set; }
		public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
	}
}