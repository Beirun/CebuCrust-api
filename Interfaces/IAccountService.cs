using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface IAccountService
    {
        Task<AuthResult> RegisterAsync(User u, string password, string confirmPassword);
        Task<AuthResult?> LoginAsync(string email, string password);
        string? Refresh(string refreshToken);
    }
}
