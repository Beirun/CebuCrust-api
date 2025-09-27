using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<Favorite>> GetByUserAsync(int uid);
        Task<bool> ExistsAsync(int uid, int pizzaId);
        Task AddFavoriteAsync(Favorite fav);
        Task<Favorite?> GetFavoriteAsync(int uid, int pizzaId);
        Task DeleteFavoriteAsync(Favorite fav);
    }
}