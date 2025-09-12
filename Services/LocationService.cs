// Services/LocationService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Config;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;

namespace CebuCrust_api.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetByUserIdAsync(int userId);
        Task<Location> CreateAsync(Location loc);
        Task<Location?> UpdateAsync(int id, Location loc);
        Task<bool> DeleteAsync(int id);
    }

    public class LocationService : ILocationService
    {
        private readonly AppDbContext _db;
        public LocationService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Location>> GetByUserIdAsync(int userId) =>
            await _db.Locations.Include(l => l.User)
                               .AsNoTracking()
                               .Where(l => l.UserId == userId)
                               .ToListAsync();

        public async Task<Location> CreateAsync(Location loc)
        {
            loc.DateCreated = DateTime.UtcNow;
            _db.Locations.Add(loc);
            await _db.SaveChangesAsync();
            return loc;
        }

        public async Task<Location?> UpdateAsync(int id, Location loc)
        {
            var existing = await _db.Locations.FindAsync(id);
            if (existing == null) return null;

            existing.LocationCity = loc.LocationCity;
            existing.LocationBrgy = loc.LocationBrgy;
            existing.LocationStreet = loc.LocationStreet;
            existing.LocationHouseNo = loc.LocationHouseNo;
            existing.DateUpdated = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Locations.FindAsync(id);
            if (existing == null) return false;

            _db.Locations.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
