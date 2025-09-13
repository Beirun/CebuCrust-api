using CebuCrust_api.Config;
using CebuCrust_api.Controllers;
using CebuCrust_api.Models;
using CebuCrust_api.Interfaces;
using CebuCrust_api.ServiceModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CebuCrust_api.Services
{
    public class RatingService : IRatingService
    {
        private readonly AppDbContext _db;
        public RatingService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Rating>> GetAllAsync() =>
            await _db.Ratings.Include(r => r.User)
                             .Include(r => r.Pizza)
                             .AsNoTracking()
                             .ToListAsync();

        public async Task<Rating?> GetByIdAsync(int id) =>
            await _db.Ratings.Include(r => r.User)
                             .Include(r => r.Pizza)
                             .AsNoTracking()
                             .FirstOrDefaultAsync(r => r.RatingId == id);

        public async Task<Rating> CreateAsync(RatingRequest request)
        {
            var r = new Rating
            {
                UserId = request.UserId,
                PizzaId = request.PizzaId,
                RatingValue = request.RatingValue,
                RatingMessage = request.RatingMessage,
                DateCreated = DateTime.UtcNow
            };

            _db.Ratings.Add(r);
            await _db.SaveChangesAsync();
            return r;
        }

        public async Task<Rating?> UpdateAsync(int id, RatingRequest request)
        {
            var existing = await _db.Ratings.FindAsync(id);
            if (existing == null) return null;

            existing.RatingValue = request.RatingValue;
            existing.RatingMessage = request.RatingMessage;
            existing.DateUpdated = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Ratings.FindAsync(id);
            if (existing == null) return false;

            _db.Ratings.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
   
}
