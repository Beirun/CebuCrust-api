using CebuCrust_api.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CebuCrust_api.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetByUserAsync(int userId);
        Task<IEnumerable<OrderResponse>> GetAllAsync(); // Admin only
        Task<OrderResponse> CreateAsync(int userId, OrderRequest request);
        Task<OrderResponse?> UpdateStatusAsync(int orderId, string status);
        Task<bool> DeleteAsync(int orderId);
    }
}
