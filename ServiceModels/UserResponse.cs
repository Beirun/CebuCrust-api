namespace CebuCrust_api.ServiceModels
{
    public class UserResponse { 
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNo { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
