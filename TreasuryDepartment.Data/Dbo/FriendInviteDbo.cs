namespace TreasureDepartment.Data.Dbo
{
    public class FriendInviteDbo : UsersOfferDbo
    {
        public FriendInviteDbo(long senderUserId, long targetUserId)
        {
            SenderUserId = targetUserId < senderUserId ? targetUserId : senderUserId;
            TargetUserId = senderUserId > targetUserId ? senderUserId : targetUserId;
        }
    }
}