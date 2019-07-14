using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TreasuryDepartment.Models
{
	public class User
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public ICollection<Friend> OutcomeFriends { get; set; } = new List<Friend>();
		public ICollection<Friend> IncomeFriends { get; set; } = new List<Friend>();
		public ICollection<BlackList> OutcomeBlackLists { get; set; } = new List<BlackList>();
		public ICollection<BlackList> IncomeBlackLists { get; set; } = new List<BlackList>();


		public ICollection<Invite> SentInvites { get; set; } = new HashSet<Invite>();
		public ICollection<Invite> ReciviedInvites { get; set; } = new HashSet<Invite>();

		public ICollection<Balance> OutcomeBalances { get; set; } = new HashSet<Balance>();
		public ICollection<Balance> IncomeBalances { get; set; } = new HashSet<Balance>();

		public ICollection<Deal> SentDeals { get; set; } = new HashSet<Deal>();
		public ICollection<Deal> ReciviedDeals { get; set; } = new HashSet<Deal>();
	}

}