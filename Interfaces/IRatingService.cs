using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface IRatingService
    {
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<Rating?> GetByIdAsync(int id);
        Task<Rating> CreateAsync(RatingRequest request);
        Task<Rating?> UpdateAsync(int id, RatingRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
