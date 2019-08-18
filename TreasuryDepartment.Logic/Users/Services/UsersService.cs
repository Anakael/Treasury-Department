using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TreasureDepartment.Data;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Logic.Users.Models;

namespace TreasureDepartment.Logic.Users.Services
{
    public class UserService
    {
        private readonly TreasuryDepartmentContext _context;
        private readonly IMapper _mapper;

        public UserService(TreasuryDepartmentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDbo> Get(long id) =>
            (await _context.Users.FindAsync(id));

        public async Task<UserDbo[]> Find(Expression<Func<UserDbo, bool>> predicate) =>
            await _context.Users.Where(predicate).ToArrayAsync();

        public async Task<UserDbo> Register(UserRegisterRequest userRegisterRequest)
        {
            var userDbo = _mapper.Map<UserDbo>(userRegisterRequest);
            userDbo.Salt = CryptographyProcessor.Services.CryptographyProcessor.CreateSalt();
            userDbo.HashedPassword =
                CryptographyProcessor.Services.CryptographyProcessor.GenerateHash(userRegisterRequest.Password,
                    userDbo.Salt);
            _context.Users.Add(userDbo);
            await _context.SaveChangesAsync();
            return userDbo;
        }
    }
}