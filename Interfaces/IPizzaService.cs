using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface IPizzaService
    {
        Task<IEnumerable<PizzaResponse>> GetAllAsync();
        Task<PizzaResponse?> GetByIdAsync(int id);
        Task<Pizza> CreateAsync(Pizza p);
        Task<Pizza?> UpdateAsync(int id, Pizza p);
        Task<bool> DeleteAsync(int id);
        Task SaveImageAsync(int pizzaId, IFormFile file);
    }
}
