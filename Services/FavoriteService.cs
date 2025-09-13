using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Config;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using CebuCrust_api.Interfaces;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Services
{

    public class FavoriteService : IFavoriteService
    {
        private readonly AppDbContext _db;
        public FavoriteService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId) =>
            await _db.Favorites.Include(f => f.Pizza)
                               .AsNoTracking()
                               .Where(f => f.UserId == userId)
                               .ToListAsync();

        public async Task<Favorite> CreateAsync(FavoriteRequest request)
        {
            var fav = new Favorite
            {
                UserId = request.UserId,
                PizzaId = request.PizzaId,
                DateCreated = DateTime.UtcNow
            };

            _db.Favorites.Add(fav);
            await _db.SaveChangesAsync();
            return fav;
        }

        public async Task<bool> DeleteAsync(int userId, int pizzaId)
        {
            var existing = await _db.Favorites.FindAsync(userId, pizzaId);
            if (existing == null) return false;

            _db.Favorites.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
    
}
