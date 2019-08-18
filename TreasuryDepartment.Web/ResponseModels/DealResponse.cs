using System;
using TreasureDepartment.Data.Dbo;

namespace TreasureDepartment.Web.ResponseModels
{
    public class DealResponse
    {
        public UserDbo UserDbo { get; set; }
        public decimal Sum { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastStatusChangeDate { get; set; }

        public DealResponse(DealDbo dealDbo, ChooseFriendUserChoice choice)
        {
            UserDbo = choice == ChooseFriendUserChoice.Sender ? dealDbo.SenderUser : dealDbo.TargetUser;
            Sum = dealDbo.Sum;
            CreatedDate = dealDbo.CreatedDate;
            LastStatusChangeDate = dealDbo.LastStatusChangeDate;
        }
    }
}