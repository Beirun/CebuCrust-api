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
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repo;
        public RatingService(IRatingRepository repo) => _repo = repo;

        private RatingResponse ToResponse(Rating r) => new RatingResponse
        {
            UserId = r.UserId,
            PizzaId = r.PizzaId,
            RatingValue = r.RatingValue,
            RatingMessage = r.RatingMessage
        };

        public async Task<IEnumerable<RatingResponse>> GetAllAsync()
        {
            var ratings = await _repo.GetAllAsync();
            return ratings.Select(r => ToResponse(r));
        }

        public async Task<RatingResponse?> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            return r == null ? null : ToResponse(r);
        }

        public async Task<IEnumerable<RatingResponse>> GetByPizzaIdAsync(int pizzaId)
        {
            var ratings = await _repo.GetByPizzaIdAsync(pizzaId);
            return ratings.Select(r => ToResponse(r));
        }

        public async Task<RatingResponse> CreateAsync(int userId, RatingRequest request)
        {
            var r = new Rating
            {
                UserId = userId,
                PizzaId = request.PizzaId,
                RatingValue = request.RatingValue,
                RatingMessage = request.RatingMessage,
                DateCreated = DateTime.UtcNow
            };
            await _repo.AddAsync(r);
            return ToResponse(r);
        }

        public async Task<RatingResponse?> UpdateAsync(int id, RatingRequest request)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.RatingValue = request.RatingValue;
            existing.RatingMessage = request.RatingMessage;
            existing.DateUpdated = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
            return ToResponse(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            return await _repo.DeleteAsync(existing);
        }
    }
}
