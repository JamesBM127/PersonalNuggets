using System.Security.Cryptography;

namespace JBMSecurePassword
{
    public class PasswordManager
    {
        public static string GenerateSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool AuthenticatePassword(string password, string salt, string hashedPassword)
        {
            string hashedInput = HashPassword(password, salt);
            return hashedInput == hashedPassword;
        }
    }
}
