using TreasuryDepartment.Models.Enums;
using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class Offer : UsersOffer
    {
        public Status Status { get; set; } = Status.Pending;
        public decimal Sum { get; set; }

        protected Offer()
        {
        }

        public Offer(RequestUsersOffer offer, decimal sum)
        {
            SenderUserId = offer.SenderUserId;
            TargetUserId = offer.TargetUserId;
            Sum = sum;
        }
    }
}