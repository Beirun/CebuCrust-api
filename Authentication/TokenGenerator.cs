using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CebuCrust_api.Models;

namespace CebuCrust_api.Authentication
{
    public class TokenGenerator
    {
        private readonly TokenOptions opt;
        private readonly SymmetricSecurityKey key;

        public TokenGenerator(TokenOptions options)
        {
            opt = options;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Key));
        }

        // Builds access token including email and role automatically
        public string CreateAccessToken(User u)
        {
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, u.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, u.UserEmail),
                new Claim(ClaimTypes.Role, u.Role?.RoleName ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: opt.Issuer,
                audience: opt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(opt.AccessExpireMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Refresh token remains userId only
        public string CreateRefreshToken(int uid)
        {
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, uid.ToString()),
                new Claim("typ","refresh")
            };

            var token = new JwtSecurityToken(
                issuer: opt.Issuer,
                audience: opt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(opt.RefreshExpireDays),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
