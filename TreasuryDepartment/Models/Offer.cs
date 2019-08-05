using TreasuryDepartment.Models.Enums;
using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class Offer : RequestUsersOffer
    {
        public User SenderUser { get; set; }
        public User TargetUser { get; set; }
        public Status Status { get; set; } = Status.Pending;

        protected Offer()
        {
        }

        public Offer(RequestUsersOffer offer)
        {
            SenderUserId = offer.SenderUserId;
            TargetUserId = offer.TargetUserId;
        }

        protected Offer(Offer other)
        {
            SenderUserId = other.SenderUserId;
            SenderUser = other.SenderUser;
            TargetUserId = other.TargetUserId;
            SenderUser = other.SenderUser;
            Status = other.Status;
        }
    }
}