using CebuCrust_api.Config;
using CebuCrust_api.Models;
using CebuCrust_api.Interfaces;
using CebuCrust_api.ServiceModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CebuCrust_api.Services
{


    public class LocationService : ILocationService
    {
        private readonly AppDbContext _db;
        public LocationService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Location>> GetByUserIdAsync(int userId) =>
            await _db.Locations.Include(l => l.User)
                               .AsNoTracking()
                               .Where(l => l.UserId == userId)
                               .ToListAsync();

        public async Task<Location> CreateAsync(LocationRequest request)
        {
            var loc = new Location
            {
                UserId = request.UserId,
                LocationCity = request.LocationCity,
                LocationBrgy = request.LocationBrgy,
                LocationStreet = request.LocationStreet,
                LocationHouseNo = request.LocationHouseNo,
                DateCreated = DateTime.UtcNow
            };

            _db.Locations.Add(loc);
            await _db.SaveChangesAsync();
            return loc;
        }

        public async Task<Location?> UpdateAsync(int id, LocationRequest request)
        {
            var existing = await _db.Locations.FindAsync(id);
            if (existing == null) return null;

            existing.UserId = request.UserId;
            existing.LocationCity = request.LocationCity;
            existing.LocationBrgy = request.LocationBrgy;
            existing.LocationStreet = request.LocationStreet;
            existing.LocationHouseNo = request.LocationHouseNo;
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
