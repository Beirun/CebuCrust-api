// Services/OrderService.cs
using CebuCrust_api.Controllers;
using CebuCrust_api.Data;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CebuCrust_api.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetAllAsync(); // Admin only
        Task<Order> CreateAsync(Order order, IEnumerable<OrderItemRequest> items);
        Task<Order?> UpdateStatusAsync(int orderId, string status);
        Task<bool> DeleteAsync(int orderId);
    }

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;
        public OrderService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId) =>
            await _db.Orders.Include(o => o.Location)
                            .Include(o => o.OrderLists).ThenInclude(ol => ol.Pizza)
                            .AsNoTracking()
                            .Where(o => o.UserId == userId)
                            .ToListAsync();

        public async Task<IEnumerable<Order>> GetAllAsync() =>
            await _db.Orders.Include(o => o.Location)
                            .Include(o => o.OrderLists).ThenInclude(ol => ol.Pizza)
                            .Include(o => o.User)
                            .AsNoTracking()
                            .ToListAsync();

        public async Task<Order> CreateAsync(Order order, IEnumerable<OrderItemRequest> items)
        {
            order.DateCreated = DateTime.UtcNow;
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Map OrderItemRequest to OrderList internally
            foreach (var item in items)
            {
                var orderList = new OrderList
                {
                    OrderId = order.OrderId,
                    PizzaId = item.PizzaId,
                    Quantity = item.Quantity
                };
                _db.OrderLists.Add(orderList);
            }

            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateStatusAsync(int orderId, string status)
        {
            var existing = await _db.Orders.FindAsync(orderId);
            if (existing == null) return null;

            existing.OrderStatus = status;
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            var existing = await _db.Orders.FindAsync(orderId);
            if (existing == null) return false;

            // Remove related OrderLists first
            var items = await _db.OrderLists.Where(ol => ol.OrderId == orderId).ToListAsync();
            _db.OrderLists.RemoveRange(items);

            _db.Orders.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
    public class OrderRequest
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string? OrderInstruction { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderEstimate { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class OrderItemRequest
    {
        public int PizzaId { get; set; }
        public int Quantity { get; set; }
    }
}
