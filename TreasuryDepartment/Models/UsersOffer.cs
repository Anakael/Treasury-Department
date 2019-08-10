using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class UsersOffer : RequestUsersOffer
    {
        public User SenderUser { get; set; }
        public User TargetUser { get; set; }
    }
}