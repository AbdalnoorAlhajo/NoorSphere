using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;

namespace Database.Utils
{
    static public class UserService
    {
        /// <summary>
        /// Extract UserID from Token.
        /// </summary>
        /// <param name="token">The Token to extract UserId from.</param>
        /// <returns>the UserID extracted from token.</returns>
        static public string? ExtractUserIDFromToken(string? token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token)?.Claims?.FirstOrDefault(c => c.Type.Contains("id"))?.Value;
        }
}
}
