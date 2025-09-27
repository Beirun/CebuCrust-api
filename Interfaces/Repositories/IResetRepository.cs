using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface IResetRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddResetAsync(Reset reset);
        Task<Reset?> GetValidResetAsync(string resetCode);
        Task<User?> GetUserByIdAsync(int userId);
        Task UpdateAsync(params object[] entities);
    }
}