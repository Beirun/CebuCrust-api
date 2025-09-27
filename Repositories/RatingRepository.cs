using CebuCrust_api.Config;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _db;
        public RatingRepository(AppDbContext db) => _db = db;

        public async Task<List<Rating>> GetAllAsync() =>
            await _db.Ratings.AsNoTracking().ToListAsync();

        public async Task<Rating?> GetByIdAsync(int id) =>
            await _db.Ratings.AsNoTracking().FirstOrDefaultAsync(r => r.RatingId == id);

        public async Task<List<Rating>> GetByPizzaIdAsync(int pizzaId) =>
            await _db.Ratings.AsNoTracking()
                             .Where(r => r.PizzaId == pizzaId)
                             .ToListAsync();

        public async Task<Rating> AddAsync(Rating r)
        {
            _db.Ratings.Add(r);
            await _db.SaveChangesAsync();
            return r;
        }

        public async Task<Rating?> UpdateAsync(Rating r)
        {
            _db.Ratings.Update(r);
            await _db.SaveChangesAsync();
            return r;
        }

        public async Task<bool> DeleteAsync(Rating r)
        {
            _db.Ratings.Remove(r);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
