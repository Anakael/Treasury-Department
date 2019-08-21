using TreasureDepartment.Data.Dbo;

namespace TreasureDepartment.Web.ResponseModels
{
    public class FriendBalanceResponse
    {
        public decimal Sum { get; set; }
        public UserDbo UserDbo { get; set; }

        public FriendBalanceResponse(FriendInviteDbo friend, ChooseFriendUserChoice choice)
        {
            Sum = friend.Sum;
            UserDbo = choice == ChooseFriendUserChoice.Sender ? friend.SenderUser : friend.TargetUser;
        }
    }
}