namespace TreasuryDepartment.Models
{
	public class Friend
	{
		public long User1Id { get; set; }
		public User User1 { get; set; }
		public long User2Id { get; set; }
		public User User2 { get; set; }
	}
}