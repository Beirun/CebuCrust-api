using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetByUserAsync(int uid);
        Task<Location?> GetByIdAsync(int uid, int id);
        Task AddLocationAsync(Location loc);
        Task UpdateLocationAsync(Location loc);
        Task DeleteLocationAsync(Location loc);
    }
}