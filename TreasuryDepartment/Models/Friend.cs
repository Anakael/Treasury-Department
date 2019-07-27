namespace TreasuryDepartment.Models
{
	public class Friend
	{
		public long User1Id { get; set; }
		public User User1 { get; set; }
		public long User2Id { get; set; }
		public User User2 { get; set; }

		public Friend(long user1Id, long user2Id)
		{
			User1Id = user1Id < user2Id ? user1Id : user2Id;
			User2Id = user2Id > user1Id ? user2Id : user1Id;
		}
	}
}