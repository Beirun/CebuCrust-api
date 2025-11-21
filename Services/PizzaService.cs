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
        private readonly IValidationService _v;

        public PizzaService(IPizzaRepository repo, IWebHostEnvironment env, IValidationService v)
        {
            _repo = repo;
            _env = env;
            _v = v;
        }

        public async Task<PizzaResponse> CreateAsync(PizzaRequest request)
        {
            Console.WriteLine($"{request.PizzaName}");
            var existing = await _repo.GetByNameAsync(request.PizzaName);
            Console.WriteLine($"{System.Text.Json.JsonSerializer.Serialize(existing)}");
            if(existing != null) throw new Exception("Pizza already exists.");
            if(request.PizzaPrice < 1) throw new Exception("Price should not be less than zero");
            if(request.Stock < 1) throw new Exception("Stock should not be less than zero");
            if(!await _v.IsValidImageAsync(request.Image!)) 
                throw new Exception("Invalid file. Must be an image and a maximum of 5MB");
            var p = new Pizza
            {
                PizzaName = request.PizzaName,
                PizzaDescription = request.PizzaDescription,
                PizzaCategory = request.PizzaCategory,
                Stock = request.Stock,
                PizzaPrice = request.PizzaPrice,
                DateCreated = DateTime.UtcNow
            };

            p = await _repo.AddAsync(p);
            return await GetByIdAsync(p.PizzaId) ?? new PizzaResponse();
        }

        public async Task<PizzaResponse?> UpdateAsync(int id, PizzaRequest request)
        {
            var p = await _repo.GetByIdAsync(id);
            var existing = await _repo.GetByNameAsync(request.PizzaName);
            if(existing != null && p!.PizzaName != existing.PizzaName) throw new Exception("Pizza already exists.");
            if (p == null) return null;
            if(request.PizzaPrice < 1) throw new Exception("Price should not be less than zero");
            Console.WriteLine(request.Stock);
            if(request.Stock < 1) throw new Exception("Stock should not be less than zero");
            if(!await _v.IsValidImageAsync(request.Image!)) 
                throw new Exception("Invalid file. Must be an image and a maximum of 5MB");
            
            p.PizzaName = request.PizzaName;
            p.PizzaDescription = request.PizzaDescription;
            p.PizzaCategory = request.PizzaCategory;
            p.PizzaPrice = request.PizzaPrice;
            p.Stock = request.Stock;
            p.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateAsync(p);
            if (request.Image != null)
                await SaveImageAsync(id, request.Image);
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
                Stock = p.Stock,
                IsDeleted = p.DateDeleted != null,
                PizzaPrice = p.PizzaPrice,
                pizzaImage = imgData
            };
        }

        public async Task SaveImageAsync(int pizzaId, IFormFile file)
        {
            if (file == null || file.Length == 0) return;
                
            var pizzasFolder = Path.Combine(_env.ContentRootPath, "Resources", "Pizzas");
            if (!Directory.Exists(pizzasFolder))
                Directory.CreateDirectory(pizzasFolder);

            var existingFiles = Directory.GetFiles(pizzasFolder, $"{pizzaId}.*");
            foreach (var existingFile in existingFiles)
            {
                try
                {
                    File.Delete(existingFile);
                }
                catch (Exception) { }
                
            }

            var ext = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(pizzasFolder, $"{pizzaId}{ext}");

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

        }

    }
}
