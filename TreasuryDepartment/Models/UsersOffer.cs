using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class UsersOffer : RequestUsersOffer
    {
        public User SenderUser { get; set; }
        public User TargetUser { get; set; }

        public UsersOffer()
        {
        }

        public UsersOffer(RequestUsersOffer offer)
        {
            SenderUserId = offer.SenderUserId;
            TargetUserId = offer.TargetUserId;
        }

        protected UsersOffer(Offer other)
        {
            SenderUserId = other.SenderUserId;
            SenderUser = other.SenderUser;
            TargetUserId = other.TargetUserId;
            SenderUser = other.SenderUser;
        }
    }
}