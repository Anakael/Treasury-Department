namespace TreasuryDepartment.Models
{
    public class BlackList
    {
        public long SenderUserId { get; set; }
        public User SenderUser { get; set; }
        public long TargetUserId { get; set; }
        public User TargetUser { get; set; }
    }
}