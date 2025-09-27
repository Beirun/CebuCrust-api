using Microsoft.Extensions.Configuration;

namespace CebuCrust_api.Authentication
{
    public static class JwtSettings
    {
        public static TokenOptions Load(IConfiguration cfg)
        {
            return new TokenOptions
            {
                Key = cfg["Jwt:Key"] ?? throw new Exception("Missing Jwt:Key"),
                Issuer = cfg["Jwt:Issuer"] ?? throw new Exception("Missing Jwt:Issuer"),
                Audience = cfg["Jwt:Audience"] ?? throw new Exception("Missing Jwt:Audience"),
                AccessExpireMinutes = int.TryParse(cfg["Jwt:ExpireMinutes"], out var m) ? m : 60,
                RefreshExpireDays = int.TryParse(cfg["Jwt:RefreshDays"], out var d) ? d : 7
            };
        }
    }
}
