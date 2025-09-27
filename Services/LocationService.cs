using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using CebuCrust_api.Repositories;
using CebuCrust_api.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repo;
        public LocationService(ILocationRepository repo) => _repo = repo;

        public async Task<IEnumerable<LocationResponse>> GetByUserAsync(int uid)
        {
            var locs = await _repo.GetByUserAsync(uid);
            return locs.Select(l => new LocationResponse
            {
                LocationId = l.LocationId,
                LocationCity = l.LocationCity,
                LocationBrgy = l.LocationBrgy,
                LocationStreet = l.LocationStreet,
                LocationHouseNo = l.LocationHouseNo
            });
        }

        public async Task<LocationResponse> CreateAsync(int uid, LocationRequest request)
        {
            var loc = new Location
            {
                UserId = uid,
                LocationCity = request.LocationCity,
                LocationBrgy = request.LocationBrgy,
                LocationStreet = request.LocationStreet,
                LocationHouseNo = request.LocationHouseNo,
                DateCreated = DateTime.UtcNow
            };
            await _repo.AddLocationAsync(loc);

            return new LocationResponse
            {
                LocationId = loc.LocationId,
                LocationCity = loc.LocationCity,
                LocationBrgy = loc.LocationBrgy,
                LocationStreet = loc.LocationStreet,
                LocationHouseNo = loc.LocationHouseNo
            };
        }

        public async Task<LocationResponse?> UpdateAsync(int uid, int id, LocationRequest request)
        {
            var existing = await _repo.GetByIdAsync(uid, id);
            if (existing == null) return null;

            existing.LocationCity = request.LocationCity;
            existing.LocationBrgy = request.LocationBrgy;
            existing.LocationStreet = request.LocationStreet;
            existing.LocationHouseNo = request.LocationHouseNo;
            existing.DateUpdated = DateTime.UtcNow;

            await _repo.UpdateLocationAsync(existing);

            return new LocationResponse
            {
                LocationId = existing.LocationId,
                LocationCity = existing.LocationCity,
                LocationBrgy = existing.LocationBrgy,
                LocationStreet = existing.LocationStreet,
                LocationHouseNo = existing.LocationHouseNo
            };
        }

        public async Task<bool> DeleteAsync(int uid, int id)
        {
            var existing = await _repo.GetByIdAsync(uid, id);
            if (existing == null) return false;

            await _repo.DeleteLocationAsync(existing);
            return true;
        }
    }
}
