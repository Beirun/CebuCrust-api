using CebuCrust_api.Models;

namespace CebuCrust_api.Interfaces
{
    public interface IPizzaRepository
    {
        Task<List<Pizza>> GetAllAsync();
        Task<Pizza?> GetByIdAsync(int id);
        Task<Pizza?> GetByNameAsync(string name);
        Task<int> GetPizzaFavoriteCountAsync(int pid);
        Task<Pizza> AddAsync(Pizza p);
        Task<Pizza?> UpdateAsync(Pizza p);
        Task<bool> DeleteAsync(Pizza p);
    }
}