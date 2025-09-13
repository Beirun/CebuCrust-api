using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetByUserIdAsync(int userId);
        Task<Location> CreateAsync(LocationRequest request);
        Task<Location?> UpdateAsync(int id, LocationRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
