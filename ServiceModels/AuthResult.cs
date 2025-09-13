namespace CebuCrust_api.ServiceModels
{
    public class AuthResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserResponse User {  get; set; }
    }
}
