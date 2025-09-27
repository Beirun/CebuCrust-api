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
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _repo;
        public FavoriteService(IFavoriteRepository repo) => _repo = repo;

        public async Task<IEnumerable<FavoriteResponse>> GetByUserAsync(int uid)
        {
            var favs = await _repo.GetByUserAsync(uid);
            return favs.Select(f => new FavoriteResponse { PizzaId = f.PizzaId });
        }

        public async Task<bool> CreateAsync(int uid, FavoriteRequest request)
        {
            if (await _repo.ExistsAsync(uid, request.PizzaId))
                return false;

            var fav = new Favorite
            {
                UserId = uid,
                PizzaId = request.PizzaId,
                DateCreated = DateTime.UtcNow
            };
            await _repo.AddFavoriteAsync(fav);
            return true;
        }

        public async Task<bool> DeleteAsync(int uid, int pizzaId)
        {
            var existing = await _repo.GetFavoriteAsync(uid, pizzaId);
            if (existing == null) return false;

            await _repo.DeleteFavoriteAsync(existing);
            return true;
        }
    }
}
