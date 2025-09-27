using CebuCrust_api.ServiceModels;
using Microsoft.AspNetCore.Http;

namespace CebuCrust_api.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> UpdateAsync(int id, UserUpdateRequest request);
        Task<bool> DeleteAsync(int id);
        Task SaveImageAsync(int userId, IFormFile file);
    }
}