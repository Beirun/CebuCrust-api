using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId);
        Task<Favorite> CreateAsync(FavoriteRequest request);
        Task<bool> DeleteAsync(int userId, int pizzaId);
    }
}
