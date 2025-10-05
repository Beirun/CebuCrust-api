using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using CebuCrust_api.Repositories;
using CebuCrust_api.ServiceModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly IPizzaRepository _repo;
        private readonly IWebHostEnvironment _env;

        public PizzaService(IPizzaRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<PizzaResponse> CreateAsync(PizzaRequest request)
        {
            var p = new Pizza
            {
                PizzaName = request.PizzaName,
                PizzaDescription = request.PizzaDescription,
                PizzaCategory = request.PizzaCategory,
                IsAvailable = true,
                PizzaPrice = request.PizzaPrice,
                DateCreated = DateTime.UtcNow
            };

            p = await _repo.AddAsync(p);
            return await GetByIdAsync(p.PizzaId) ?? new PizzaResponse();
        }

        public async Task<PizzaResponse?> UpdateAsync(int id, PizzaRequest request)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            p.PizzaName = request.PizzaName;
            p.PizzaDescription = request.PizzaDescription;
            p.PizzaCategory = request.PizzaCategory;
            p.PizzaPrice = request.PizzaPrice;
            p.DateUpdated = DateTime.UtcNow;
            p.IsAvailable = request.IsAvailable;
            await _repo.UpdateAsync(p);
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return false;
            return await _repo.DeleteAsync(p);
        }

        public async Task<IEnumerable<PizzaResponse>> GetAllAsync()
        {
            var pizzas = await _repo.GetAllAsync();
            return pizzas.Select(p => ToResponse(p));
        }

        public async Task<PizzaResponse?> GetByIdAsync(int id)
        {
            var pizza = await _repo.GetByIdAsync(id);
            return pizza == null ? null : ToResponse(pizza);
        }

        private PizzaResponse ToResponse(Pizza p)
        {
            byte[]? imgData = null;
            var folder = Path.Combine(_env.ContentRootPath, "Resources", "Pizzas");
            if (Directory.Exists(folder))
            {
                var file = Directory.GetFiles(folder, p.PizzaId + ".*").FirstOrDefault();
                if (file != null) imgData = File.ReadAllBytes(file);
            }

            return new PizzaResponse
            {
                PizzaId = p.PizzaId,
                PizzaName = p.PizzaName,
                PizzaDescription = p.PizzaDescription ?? "",
                PizzaCategory = p.PizzaCategory ?? "",
                IsAvailable = p.IsAvailable,
                PizzaPrice = p.PizzaPrice,
                pizzaImage = imgData
            };
        }

        public async Task SaveImageAsync(int pizzaId, IFormFile file)
        {
            if (file == null || file.Length == 0) return;

            var pizzasFolder = Path.Combine(_env.ContentRootPath, "Resources", "Pizzas");
            if (!Directory.Exists(pizzasFolder)) Directory.CreateDirectory(pizzasFolder);

            var ext = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(pizzasFolder, pizzaId + ext);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}
