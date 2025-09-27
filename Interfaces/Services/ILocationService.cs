using CebuCrust_api.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CebuCrust_api.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationResponse>> GetByUserAsync(int userId);
        Task<LocationResponse> CreateAsync(int userId, LocationRequest request);
        Task<LocationResponse?> UpdateAsync(int userId, int id, LocationRequest request);
        Task<bool> DeleteAsync(int userId, int id);
    }
}
