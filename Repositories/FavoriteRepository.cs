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
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _db;
        public FavoriteRepository(AppDbContext db) => _db = db;

        public async Task<List<Favorite>> GetByUserAsync(int uid) =>
            await _db.Favorites.Where(f => f.UserId == uid)
                               .ToListAsync();

        public async Task<bool> ExistsAsync(int uid, int pizzaId) =>
            await _db.Favorites.AnyAsync(f => f.UserId == uid && f.PizzaId == pizzaId);

        public async Task AddFavoriteAsync(Favorite fav)
        {
            _db.Favorites.Add(fav);
            await _db.SaveChangesAsync();
        }

        public async Task<Favorite?> GetFavoriteAsync(int uid, int pizzaId) =>
            await _db.Favorites.FindAsync(uid, pizzaId);

        public async Task DeleteFavoriteAsync(Favorite fav)
        {
            _db.Favorites.Remove(fav);
            await _db.SaveChangesAsync();
        }
    }
}
