using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetAllAsync(); // Admin only
        Task<Order> CreateAsync(Order order, IEnumerable<OrderItemRequest> items);
        Task<Order?> UpdateStatusAsync(int orderId, string status);
        Task<bool> DeleteAsync(int orderId);
    }
}
