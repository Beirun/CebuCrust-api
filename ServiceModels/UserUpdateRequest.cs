namespace CebuCrust_api.ServiceModels
{
    public class UserUpdateRequest
    {
        public string? UserFName { get; set; }
        public string? UserLName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNo { get; set; }

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }

        public IFormFile? Image { get; set; }
    }
}
