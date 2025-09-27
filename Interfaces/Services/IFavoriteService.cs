using CebuCrust_api.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CebuCrust_api.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteResponse>> GetByUserAsync(int userId);
        Task<bool> CreateAsync(int userId, FavoriteRequest request);
        Task<bool> DeleteAsync(int userId, int pizzaId);
    }
}
