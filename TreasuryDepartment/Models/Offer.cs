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

        public Offer(Offer other) : base(other)
        {
            Status = other.Status;
        }
    }
}