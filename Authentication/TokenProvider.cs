using System.IdentityModel.Tokens.Jwt;
using CebuCrust_api.Models;

namespace CebuCrust_api.Authentication
{
    public class TokenProvider
    {
        private readonly TokenGenerator gen;
        private readonly TokenValidator val;

        public TokenProvider(TokenOptions opt)
        {
            gen = new TokenGenerator(opt);
            val = new TokenValidator(opt);
        }

        public string CreateAccess(User u) => gen.CreateAccessToken(u);
        public string CreateRefresh(int uid) => gen.CreateRefreshToken(uid);
        public JwtSecurityToken? Validate(string token) => val.Validate(token);
    }
}
