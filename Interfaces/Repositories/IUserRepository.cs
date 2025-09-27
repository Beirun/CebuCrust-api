using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User u);
        Task<bool> SoftDeleteAsync(User u);
    }
}