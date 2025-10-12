using CebuCrust_api.Config;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;
        public OrderRepository(AppDbContext db) => _db = db;

        public async Task<List<Order>> GetByUserAsync(int uid) =>
            await _db.Orders
                     .Include(o => o.OrderLists)
                     .ThenInclude(ol => ol.Pizza)
                     .Include(o => o.User)
                     .AsNoTracking()
                     .Where(o => o.UserId == uid)
                     .ToListAsync();

        public async Task<List<Order>> GetAllAsync() =>
            await _db.Orders
                     .Include(o => o.OrderLists)
                     .ThenInclude(ol => ol.Pizza)
                     .Include(o => o.Location)
                     .Include(o => o.User)
                     .AsNoTracking()
                     .ToListAsync();

        public async Task<Order> AddOrderAsync(Order order, IEnumerable<OrderList> items)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var item in items)
            {
                item.OrderId = order.OrderId;
                _db.OrderLists.Add(item);
            }
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetByIdAsync(int orderId) =>
            await _db.Orders.FindAsync(orderId);

        public async Task<List<OrderList>> GetOrderItemsAsync(int orderId) =>
            await _db.OrderLists
                     .Where(ol => ol.OrderId == orderId)
                     .ToListAsync();

        public async Task UpdateOrderAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order, IEnumerable<OrderList> items)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            
            var existingItems = await _db.OrderLists.Where(ol => ol.OrderId == order.OrderId).ToListAsync();
            _db.OrderLists.RemoveRange(existingItems);
            await _db.SaveChangesAsync();

            foreach (var item in items)
            {
                item.OrderId = order.OrderId;
                _db.OrderLists.Add(item);
            }
            await _db.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            var items = await _db.OrderLists.Where(ol => ol.OrderId == order.OrderId).ToListAsync();
            _db.OrderLists.RemoveRange(items);
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
        }
    }
}
