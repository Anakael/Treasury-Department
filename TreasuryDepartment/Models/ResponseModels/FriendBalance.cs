namespace TreasuryDepartment.Models.ResponseModels
{
    public class FriendBalance
    {
        public decimal Sum { get; set; }
        public User User { get; set; }

        public FriendBalance(FriendInvite friend, ChooseFriendUserChoice choice)
        {
            Sum = friend.Sum;
            User = choice == ChooseFriendUserChoice.Sender ? friend.SenderUser : friend.TargetUser;
        }
    }
}