using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TreasuryDepartment.Models
{
	public class User
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
		public ICollection<Invite> ReciviedInvites { get; set; } = new HashSet<Invite>();

		public ICollection<Balance> OutcomeBalances { get; set; } = new HashSet<Balance>();
		public ICollection<Balance> IncomeBalances { get; set; } = new HashSet<Balance>();

		public ICollection<Deal> SentDeals { get; set; } = new HashSet<Deal>();
		public ICollection<Deal> ReciviedDeals { get; set; } = new HashSet<Deal>();
	}

}