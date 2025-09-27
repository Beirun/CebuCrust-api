using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CebuCrust_api.Authentication
{
    public class TokenValidator
    {
        private readonly TokenOptions opt;
        private readonly SymmetricSecurityKey key;

        public TokenValidator(TokenOptions options)
        {
            opt = options;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Key));
        }

        public JwtSecurityToken? Validate(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = opt.Issuer,
                    ValidAudience = opt.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out var validated);

                return (JwtSecurityToken)validated;
            }
            catch { return null; }
        }
    }
}
