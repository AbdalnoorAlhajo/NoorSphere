using BCrypt.Net;

namespace Database.Utils
{
    static public class UserService
    {
        /// <summary>
        /// Hashes the provided password using BCrypt.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password as a base64 string.</returns>
        static public string HashPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while hashing the password: {ex.Message}");
                throw new Exception("Password hashing failed", ex);
            }
        }

        /// <summary>
        /// Verifies if the entered password matches the stored hashed password.
        /// </summary>
        /// <param name="enteredPassword">The password entered by the user during login.</param>
        /// <param name="storedHash">The stored hashed password from the database.</param>
        /// <returns>True if the entered password matches the stored hash, otherwise false.</returns>
        static public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while verifying the password: {ex.Message}");
                throw new Exception("Password verification failed", ex);
            }
        }
    }
}
