namespace TreasureDepartment.Data.Dbo
{
    public class UserDbo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Requisites { get; set; }
        public string Login { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}