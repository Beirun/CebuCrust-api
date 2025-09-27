using CebuCrust_api.ServiceModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CebuCrust_api.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartResponse>> GetByUserAsync(int userId);
        Task<CartResponse> CreateAsync(int userId, CartRequest request);
        Task<CartResponse?> UpdateAsync(int userId, CartRequest request);
        Task<bool> DeleteAsync(int userId, int pizzaId);

    }
}