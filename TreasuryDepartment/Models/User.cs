using System.Collections.Generic;

namespace TreasuryDepartment.Models
{
	public class User
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public ICollection<Balance> OutcomeBalances { get; set; } = new List<Balance>();
		public ICollection<Balance> IncomeBalances { get; set; } = new List<Balance>();
	}
}