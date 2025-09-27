using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingResponse>> GetAllAsync();
        Task<RatingResponse?> GetByIdAsync(int id);
        Task<IEnumerable<RatingResponse>> GetByPizzaIdAsync(int pizzaId);
        Task<RatingResponse> CreateAsync(int userId, RatingRequest request);
        Task<RatingResponse?> UpdateAsync(int id, RatingRequest request);
        Task<bool> DeleteAsync(int id);
    }
}