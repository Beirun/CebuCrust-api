using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> UpdateAsync(int id, UserUpdateRequest request);
        Task<bool> DeleteAsync(int id);
        Task SaveImageAsync(int userId, IFormFile file);
    }
}
