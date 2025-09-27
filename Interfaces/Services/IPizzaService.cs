using CebuCrust_api.ServiceModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CebuCrust_api.Interfaces
{
    public interface IPizzaService
    {
        Task<IEnumerable<PizzaResponse>> GetAllAsync();
        Task<PizzaResponse?> GetByIdAsync(int id);
        Task<PizzaResponse> CreateAsync(PizzaRequest request);
        Task<PizzaResponse?> UpdateAsync(int id, PizzaRequest request);
        Task<bool> DeleteAsync(int id);
        Task SaveImageAsync(int pizzaId, IFormFile file);
    }
}