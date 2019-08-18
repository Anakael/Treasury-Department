using AutoMapper;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Web.Users.Models;

namespace TreasureDepartment.Web.Users.Profiles
{
    public class UserResponseProfile : Profile
    {
        public UserResponseProfile()
        {
            CreateMap<UserDbo, User>();
        }
    }
}