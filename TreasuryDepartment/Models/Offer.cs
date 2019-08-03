using TreasuryDepartment.Models.Enums;
using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class Offer : RequestOffer
    {
        public User SenderUser { get; set; }
        public User TargetUser { get; set; }

        protected Offer()
        {
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