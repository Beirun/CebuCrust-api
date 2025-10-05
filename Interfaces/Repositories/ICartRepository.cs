using CebuCrust_api.Models;
namespace CebuCrust_api.Interfaces
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetByUserAsync(int uid);
        Task<Cart?> GetCartItemAsync(int uid, int pizzaId);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartItemAsync(Cart cart);
    }
}