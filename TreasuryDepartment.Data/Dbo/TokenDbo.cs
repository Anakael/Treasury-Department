namespace TreasureDepartment.Data.Dbo
{
    public class TokenDbo
    {
        public long UserId { get; set; }
        public UserDbo User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}