using TreasuryDepartment.Models.RequestModels;

namespace TreasuryDepartment.Models
{
    public class Balance : UsersOffer
    {
        public decimal Sum { get; set; }

        public Balance()
        {
        }


        public Balance(UsersOffer offer, decimal sum)
        {
            SenderUserId = offer.SenderUserId;
            SenderUser = offer.SenderUser;
            TargetUserId = offer.TargetUserId;
            TargetUser = offer.TargetUser;
            Sum = sum;
        }
    }
}