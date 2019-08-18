namespace TreasureDepartment.Data.Dbo
{
    public class BlackListDbo
    {
        public long SenderUserId { get; set; }
        public UserDbo SenderUserDbo { get; set; }
        public long TargetUserId { get; set; }
        public UserDbo TargetUserDbo { get; set; }
    }
}