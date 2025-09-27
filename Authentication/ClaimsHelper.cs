using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CebuCrust_api.Authentication
{
    public static class ClaimsHelper
    {
        /// <summary>
        /// Retrieves the current logged-in user's ID from the ClaimsPrincipal.
        /// Expects the user ID to be in the "sub" claim (JwtRegisteredClaimNames.Sub).
        /// </summary>
        public static int GetUserId(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var val = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(val))
                throw new InvalidOperationException("User ID claim missing in token");

            return int.Parse(val); // adjust if your ID is a Guid
        }

        /// <summary>
        /// Retrieves the role of the current user from the ClaimsPrincipal.
        /// </summary>
        public static string GetUserRole(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var role = user.FindFirstValue(ClaimTypes.Role);
            return string.IsNullOrEmpty(role) ? "User" : role;
        }
    }
}
