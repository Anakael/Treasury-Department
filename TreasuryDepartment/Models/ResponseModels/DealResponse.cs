using System;

namespace TreasuryDepartment.Models.ResponseModels
{
    public class DealResponse
    {
        public User User { get; set; }
        public decimal Sum { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastStatusChangeDate { get; set; }

        public DealResponse(Deal deal, ChooseFriendUserChoice choice)
        {
            User = choice == ChooseFriendUserChoice.Sender ? deal.SenderUser : deal.TargetUser;
            Sum = deal.Sum;
            CreatedDate = deal.CreatedDate;
            LastStatusChangeDate = deal.LastStatusChangeDate;
        }
    }
}