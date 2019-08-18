using System;
using System.Security.Cryptography;
using System.Text;

namespace TreasureDepartment.Logic.CryptographyProcessor.Services
{
    public static class CryptographyProcessor
    {
        private const short SaltSize = 8;

        public static string CreateSalt()
        {
            var provider = new RNGCryptoServiceProvider();
            var array = new byte[SaltSize];
            provider.GetBytes(array);
            return Convert.ToBase64String(array);
        }

        public static string GenerateHash(string password, string salt)
        {
            var array = Encoding.UTF8.GetBytes(password + salt);
            var sha256Managed = new SHA256Managed();
            var hash = sha256Managed.ComputeHash(array);
            return Convert.ToBase64String(hash);
        }

        public static bool Validate(string password, string hash, string salt) =>
            GenerateHash(password, salt) == hash;
    }
}