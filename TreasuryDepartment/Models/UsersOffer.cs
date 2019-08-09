using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class UsersOffer : RequestUsersOffer
    {
        public User SenderUser { get; set; }
        public User TargetUser { get; set; }

        protected UsersOffer()
        {
        }

        public UsersOffer(UsersOffer other)
        {
            SenderUserId = other.SenderUserId;
            SenderUser = other.SenderUser;
            TargetUserId = other.TargetUserId;
            TargetUser = other.TargetUser;
        }
    }
}