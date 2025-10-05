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
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _db;
        public CartRepository(AppDbContext db) => _db = db;

        public async Task<List<Cart>> GetByUserAsync(int uid) =>
            await _db.Carts.AsNoTracking()
                           .Where(c => c.UserId == uid)
                           .ToListAsync();

        public async Task<Cart?> GetCartItemAsync(int uid, int pizzaId) =>
            await _db.Carts.FindAsync(pizzaId, uid);

        public async Task AddCartAsync(Cart cart)
        {
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _db.Carts.Update(cart);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(Cart cart)
        {
            _db.Carts.Remove(cart);
            await _db.SaveChangesAsync();
        }
    }
}