using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using CebuCrust_api.Repositories;
using CebuCrust_api.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo) => _repo = repo;

        public async Task<IEnumerable<OrderResponse>> GetByUserAsync(int uid)
        {
            var orders = await _repo.GetByUserAsync(uid);
            return orders.Select(o => new OrderResponse
            {
                OrderId = o.OrderId,
                LocationId = o.LocationId,
                OrderInstruction = o.OrderInstruction,
                OrderStatus = o.OrderStatus,
                OrderEstimate = o.OrderEstimate,
                Items = o.OrderLists.Select(ol => new OrderItemResponse
                {
                    PizzaId = ol.PizzaId,
                    Quantity = ol.Quantity
                }).ToList()
            });
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _repo.GetAllAsync();
            return orders.Select(o => new OrderResponse
            {
                OrderId = o.OrderId,
                LocationId = o.LocationId,
                OrderInstruction = o.OrderInstruction,
                OrderStatus = o.OrderStatus,
                OrderEstimate = o.OrderEstimate,
                Items = o.OrderLists.Select(ol => new OrderItemResponse
                {
                    PizzaId = ol.PizzaId,
                    Quantity = ol.Quantity
                }).ToList()
            });
        }

        public async Task<OrderResponse> CreateAsync(int uid, OrderRequest request)
        {
            var order = new Order
            {
                UserId = uid,
                LocationId = request.LocationId,
                OrderInstruction = request.OrderInstruction,
                OrderStatus = request.OrderStatus,
                OrderEstimate = request.OrderEstimate,
                DateCreated = DateTime.UtcNow
            };

            var items = request.Items.Select(i => new OrderList
            {
                OrderId = order.OrderId, // EF will generate ID after SaveChanges, so may need to Save first
                PizzaId = i.PizzaId,
                Quantity = i.Quantity
            }).ToList();

            await _repo.AddOrderAsync(order, items);

            return new OrderResponse
            {
                OrderId = order.OrderId,
                LocationId = order.LocationId,
                OrderInstruction = order.OrderInstruction,
                OrderStatus = order.OrderStatus,
                OrderEstimate = order.OrderEstimate,
                Items = items.Select(i => new OrderItemResponse
                {
                    PizzaId = i.PizzaId,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public async Task<OrderResponse?> UpdateStatusAsync(int orderId, string status)
        {
            var existing = await _repo.GetByIdAsync(orderId);
            if (existing == null) return null;

            existing.OrderStatus = status;
            await _repo.UpdateOrderAsync(existing);

            var items = await _repo.GetOrderItemsAsync(orderId);
            return new OrderResponse
            {
                OrderId = existing.OrderId,
                LocationId = existing.LocationId,
                OrderInstruction = existing.OrderInstruction,
                OrderStatus = existing.OrderStatus,
                OrderEstimate = existing.OrderEstimate,
                Items = items.Select(i => new OrderItemResponse
                {
                    PizzaId = i.PizzaId,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            var existing = await _repo.GetByIdAsync(orderId);
            if (existing == null) return false;

            await _repo.DeleteOrderAsync(existing);
            return true;
        }
    }
}
