namespace TreasureDepartment.Logic.Tokens.Models
{
    public class AccessToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}