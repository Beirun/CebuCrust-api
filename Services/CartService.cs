// Services/CartService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Config;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;

namespace CebuCrust_api.Services
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetByUserIdAsync(int userId);
        Task<Cart> CreateAsync(Cart cart);
        Task<Cart?> UpdateAsync(int userId, int pizzaId, Cart cart);
        Task<bool> DeleteAsync(int userId, int pizzaId);
    }

    public class CartService : ICartService
    {
        private readonly AppDbContext _db;
        public CartService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Cart>> GetByUserIdAsync(int userId) =>
            await _db.Carts.Include(c => c.Pizza)
                           .AsNoTracking()
                           .Where(c => c.UserId == userId)
                           .ToListAsync();

        public async Task<Cart> CreateAsync(Cart cart)
        {
            cart.DateCreated = DateTime.UtcNow;
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart?> UpdateAsync(int userId, int pizzaId, Cart cart)
        {
            var existing = await _db.Carts.FindAsync(pizzaId, userId);
            if (existing == null) return null;

            existing.Quantity = cart.Quantity;
            existing.DateUpdated = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int userId, int pizzaId)
        {
            var existing = await _db.Carts.FindAsync(pizzaId, userId);
            if (existing == null) return false;

            _db.Carts.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
