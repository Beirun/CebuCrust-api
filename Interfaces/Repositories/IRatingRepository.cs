using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface IRatingRepository
    {
        Task<List<Rating>> GetAllAsync();
        Task<Rating?> GetByIdAsync(int id);
        Task<List<Rating>> GetByPizzaIdAsync(int pizzaId);
        Task<Rating> AddAsync(Rating r);
        Task<Rating?> UpdateAsync(Rating r);
        Task<bool> DeleteAsync(Rating r);
    }
}