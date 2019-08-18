using AutoMapper;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Logic.Users.Models;

namespace TreasureDepartment.Logic.Users.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterRequest, UserDbo>();
        }
    }
}