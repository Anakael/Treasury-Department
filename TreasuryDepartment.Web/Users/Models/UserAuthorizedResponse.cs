using TreasureDepartment.Logic.Tokens.Models;

namespace TreasureDepartment.Web.Users.Models
{
    public class UserAuthorizedResponse
    {
        public User User { get; set; }
        public AccessToken Token { get; set; }
    }
}