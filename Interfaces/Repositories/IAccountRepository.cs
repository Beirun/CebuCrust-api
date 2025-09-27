using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> IsFirstUserAsync();
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task AddUserAsync(User u);
    }
}