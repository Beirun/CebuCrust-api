namespace CebuCrust_api.ServiceModels
{
    public class PasswordResetRequest
    {
        public string ResetCode { get; set; }
        public string NewPassword {  get; set; }
        public string ConfirmPassword { get; set; }
    }
}
