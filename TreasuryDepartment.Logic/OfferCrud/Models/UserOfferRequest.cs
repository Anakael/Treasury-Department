namespace TreasureDepartment.Logic.OfferCrud.Models
{
    public class UsersOfferRequest
    {
        public long SenderUserId { get; set; }
        public long TargetUserId { get; set; }
    }
}