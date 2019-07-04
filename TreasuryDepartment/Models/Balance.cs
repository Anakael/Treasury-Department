namespace TreasuryDepartment.Models
{
	public class Balance
	{
		public decimal Sum { get; set; }
		public User From { get; set; }
		public User To { get; set; }
	}
}