namespace CebuCrust_api.Authentication
{
    public class TokenOptions
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessExpireMinutes { get; set; } = 60;
        public int RefreshExpireDays { get; set; } = 7;
    }
}
