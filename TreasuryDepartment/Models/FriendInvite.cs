namespace TreasuryDepartment.Models
{
    public class FriendInvite : Offer
    {
        public FriendInvite(long senderUserId, long targetUserId)
        {
            SenderUserId = targetUserId < senderUserId ? targetUserId : senderUserId;
            TargetUserId = senderUserId > targetUserId ? senderUserId : targetUserId;
        }
    }
}