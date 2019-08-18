using TreasureDepartment.Data.Enums;

namespace TreasureDepartment.Data.Dbo
{
    public abstract class UsersOfferDbo
    {
        public long SenderUserId { get; set; }
        public long TargetUserId { get; set; }
        public decimal Sum { get; set; }
        public UserDbo SenderUser { get; set; }
        public UserDbo TargetUser { get; set; }
        public Status Status { get; set; } = Status.Pending;
    }
}