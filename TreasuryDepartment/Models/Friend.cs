namespace TreasuryDepartment.Models
{
    public class Friend : Offer
    {
        public Friend(long senderUserId, long targetUserId)
        {
            SenderUserId = targetUserId < senderUserId ? targetUserId : senderUserId;
            TargetUserId = senderUserId > targetUserId ? senderUserId : targetUserId;
        }
    }
}