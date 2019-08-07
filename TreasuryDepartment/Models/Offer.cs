using TreasuryDepartment.Models.Enums;
using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class Offer : UsersOffer
    {
        public Status Status { get; set; } = Status.Pending;

        protected Offer()
        {
        }

        public Offer(RequestUsersOffer offer)
        {
            SenderUserId = offer.SenderUserId;
            TargetUserId = offer.TargetUserId;
        }

        public Offer(Offer other)
        {
            SenderUserId = other.SenderUserId;
            SenderUser = other.SenderUser;
            TargetUserId = other.TargetUserId;
            SenderUser = other.SenderUser;
            Status = other.Status;
        }
    }
}