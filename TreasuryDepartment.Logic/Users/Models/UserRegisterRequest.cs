namespace TreasureDepartment.Logic.Users.Models
{
    public class UserRegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}