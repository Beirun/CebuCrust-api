using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetByUserIdAsync(int userId);
        Task<Cart> CreateAsync(int userId, int pizzaId, int quantity);
        Task<Cart?> UpdateAsync(int userId, int pizzaId, int quantity);
        Task<bool> DeleteAsync(int userId, int pizzaId);
    }
}
