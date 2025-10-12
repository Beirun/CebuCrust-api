using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetByUserAsync(int uid);
        Task<List<Order>> GetAllAsync();
        Task<Order> AddOrderAsync(Order order, IEnumerable<OrderList> items);
        Task<Order?> GetByIdAsync(int orderId);
        Task<List<OrderList>> GetOrderItemsAsync(int orderId);
        Task UpdateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order, IEnumerable<OrderList> items);
        Task DeleteOrderAsync(Order order);
    }
}