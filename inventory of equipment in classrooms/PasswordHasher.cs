using BCrypt.Net;
using System.Security.Cryptography;

namespace inventory_of_equipment_in_classrooms
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12, false);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword, false);
        }
    }
}