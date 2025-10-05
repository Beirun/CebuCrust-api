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
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        public CartService(ICartRepository repo) => _repo = repo;

        public async Task<IEnumerable<CartResponse>> GetByUserAsync(int uid)
        {
            var carts = await _repo.GetByUserAsync(uid);
            return carts.Select(c => new CartResponse { PizzaId = c.PizzaId, Quantity = c.Quantity });
        }

        public async Task<CartResponse> CreateAsync(int uid, CartRequest request)
        {
            var existing = await _repo.GetCartItemAsync(uid, request.PizzaId);
            if (existing != null)
            {
                existing.Quantity += request.Quantity;
                existing.DateUpdated = DateTime.UtcNow;
                await _repo.UpdateCartAsync(existing);
                return new CartResponse { PizzaId = existing.PizzaId, Quantity = existing.Quantity };
            }

            var cart = new Cart
            {
                UserId = uid,
                PizzaId = request.PizzaId,
                Quantity = request.Quantity,
                DateCreated = DateTime.UtcNow
            };
            await _repo.AddCartAsync(cart);
            return new CartResponse { PizzaId = cart.PizzaId, Quantity = cart.Quantity };
        }

        public async Task<CartResponse?> UpdateAsync(int uid, CartRequest request)
        {
            var existing = await _repo.GetCartItemAsync(uid, request.PizzaId);
            if (existing == null) return null;

            existing.Quantity = request.Quantity;
            existing.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateCartAsync(existing);
            return new CartResponse { PizzaId = existing.PizzaId, Quantity = existing.Quantity };
        }

        public async Task<bool> DeleteCartItemAsync(int uid, int pizzaId)
        {
            var existing = await _repo.GetCartItemAsync(uid, pizzaId);
            if (existing == null) return false;

            await _repo.DeleteCartItemAsync(existing);
            return true;
        }

        public async Task<bool> DeleteCartAsync(int uid)
        {
            var carts = await _repo.GetByUserAsync(uid);
            if (carts == null || !carts.Any()) return false;

            foreach (var cart in carts)
            {
                await _repo.DeleteCartItemAsync(cart);
            }
            return true;
        }
    }
}
