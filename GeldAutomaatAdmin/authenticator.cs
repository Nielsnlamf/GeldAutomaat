using System;
using System.Security.Cryptography;

namespace Authenticator
{
    public class Authenticator
    {
        private const int Iterations = 10000;
        private const int SaltSize = 16;

        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);

            byte[] hash = pbkdf2.GetBytes(20); // 160 bits

            byte[] hashBytes = new byte[SaltSize + 20]; // 128 bits (salt) + 160 bits (hash)
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, 20);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if(password == null || hashedPassword == null)
            {
                return false;
            }
            System.Diagnostics.Debug.WriteLine("Checking: " + password + " against: \n" + hashedPassword);
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(20); // 160 bits

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
