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
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _db;
        public LocationRepository(AppDbContext db) => _db = db;

        public async Task<List<Location>> GetByUserAsync(int uid) =>
            await _db.Locations.AsNoTracking()
                               .Where(l => l.UserId == uid && l.DateDeleted == null)
                               .ToListAsync();

        public async Task<Location?> GetByIdAsync(int uid, int id) =>
            await _db.Locations.FirstOrDefaultAsync(l => l.LocationId == id && l.UserId == uid && l.DateDeleted == null);

        public async Task AddLocationAsync(Location loc)
        {
            _db.Locations.Add(loc);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateLocationAsync(Location loc)
        {
            _db.Locations.Update(loc);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(Location loc)
        {
            loc.DateDeleted = DateTime.UtcNow;
            _db.Locations.Update(loc);
            await _db.SaveChangesAsync();
        }
    }
}