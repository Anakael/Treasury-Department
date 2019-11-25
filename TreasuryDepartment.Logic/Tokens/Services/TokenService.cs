using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TreasureDepartment.Data;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Logic.Options;
using TreasureDepartment.Logic.Tokens.Models;

namespace TreasureDepartment.Logic.Tokens.Services
{
    public class TokenService
    {
        private readonly JWTOptions _jwtOptions;
        private readonly TreasuryDepartmentContext _context;

        public TokenService(JWTOptions jwtOptions, TreasuryDepartmentContext context)
        {
            _jwtOptions = jwtOptions;
            _context = context;
        }

        public AccessToken GenerateToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                now,
                now.AddMinutes(_jwtOptions.LifeTime),
                new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            return new AccessToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresIn = _jwtOptions.LifeTime,
                RefreshToken = GenerateRefreshToken()
            };
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidateParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidateParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<AccessToken> RefreshToken(string token, string refreshToken)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                var userLogin = principal.Identity.Name;
                var savedToken = await _context.Tokens.Where(t =>
                        t.User.Login == userLogin && t.Token == token && t.RefreshToken == refreshToken)
                    .SingleOrDefaultAsync();

                if (savedToken == null)
                    throw new SecurityTokenException("Invalid refresh token");

                var newJwtToken = GenerateToken(principal.Claims);
                savedToken.Token = newJwtToken.Token;
                savedToken.RefreshToken = newJwtToken.RefreshToken;
                _context.Entry(savedToken).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return newJwtToken;
            }
            catch (SecurityTokenException)
            {
                var compromatedTokens = _context.Tokens.Where(t => t.Token == token);
                foreach (var compromatedToken in compromatedTokens)
                {
                    _context.Tokens.Remove(compromatedToken);
                }

                await _context.SaveChangesAsync();
                return null;
            }
        }

        public async Task Save(TokenDbo token)
        {
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
        }
    }
}