// Services/FavoriteService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Data;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;

namespace CebuCrust_api.Services
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId);
        Task<Favorite> CreateAsync(Favorite f);
        Task<bool> DeleteAsync(int userId, int pizzaId);
    }

    public class FavoriteService : IFavoriteService
    {
        private readonly AppDbContext _db;
        public FavoriteService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId) =>
            await _db.Favorites.Include(f => f.Pizza)
                               .AsNoTracking()
                               .Where(f => f.UserId == userId)
                               .ToListAsync();

        public async Task<Favorite> CreateAsync(Favorite f)
        {
            f.DateCreated = DateTime.UtcNow;
            _db.Favorites.Add(f);
            await _db.SaveChangesAsync();
            return f;
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
