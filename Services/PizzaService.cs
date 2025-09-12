// Services/PizzaService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Data;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;

namespace CebuCrust_api.Services
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

    public class PizzaService : IPizzaService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public PizzaService(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<Pizza> CreateAsync(Pizza p)
        {
            p.DateCreated = DateTime.UtcNow;
            _db.Pizzas.Add(p);
            await _db.SaveChangesAsync();
            return p;
        }

        public async Task<Pizza?> UpdateAsync(int id, Pizza p)
        {
            var e = await _db.Pizzas.FindAsync(id);
            if (e == null) return null;

            e.PizzaName = p.PizzaName;
            e.PizzaDescription = p.PizzaDescription;
            e.PizzaCategory = p.PizzaCategory;
            e.PizzaPrice = p.PizzaPrice;
            e.DateUpdated = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return e;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _db.Pizzas.FindAsync(id);
            if (e == null) return false;

            _db.Pizzas.Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PizzaResponse>> GetAllAsync()
        {
            var pizzas = await _db.Pizzas.AsNoTracking().ToListAsync();
            return pizzas.ConvertAll(p => ToResponse(p));
        }

        public async Task<PizzaResponse?> GetByIdAsync(int id)
        {
            var pizza = await _db.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == id);
            return pizza == null ? null : ToResponse(pizza);
        }

        private PizzaResponse ToResponse(Pizza p)
        {
            var pizzasFolder = Path.Combine(_env.ContentRootPath, "Resources", "Pizzas");
            string? imageUrl = null;

            // Look for any file in Resources starting with PizzaId
            if (Directory.Exists(pizzasFolder))
            {
                var file = Directory.GetFiles(pizzasFolder, p.PizzaId + ".*").FirstOrDefault();
                if (file != null)
                {
                    var ext = Path.GetExtension(file);
                    imageUrl = $"/Resources/Pizzas/{p.PizzaId}{ext}";
                }
            }

            return new PizzaResponse
            {
                PizzaId = p.PizzaId,
                PizzaName = p.PizzaName,
                PizzaDescription = p.PizzaDescription,
                PizzaCategory = p.PizzaCategory,
                PizzaPrice = p.PizzaPrice,
                ImageUrl = imageUrl
            };
        }
        public async Task SaveImageAsync(int pizzaId, IFormFile file)
        {
            if (file == null || file.Length == 0) return;

            var pizzasFolder = Path.Combine(_env.ContentRootPath, "Resources", "Pizzas");
            if (!Directory.Exists(pizzasFolder)) Directory.CreateDirectory(pizzasFolder);

            var ext = Path.GetExtension(file.FileName); // preserve extension
            var filePath = Path.Combine(pizzasFolder, pizzaId + ext);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
    public class PizzaCreateRequest
    {
        public string PizzaName { get; set; } = "";
        public string PizzaDescription { get; set; } = "";
        public string PizzaCategory { get; set; } = "";
        public decimal PizzaPrice { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class PizzaUpdateRequest
    {
        public string PizzaName { get; set; } = "";
        public string PizzaDescription { get; set; } = "";
        public string PizzaCategory { get; set; } = "";
        public decimal PizzaPrice { get; set; }
        public IFormFile? Image { get; set; }
    }
    public class PizzaResponse
    {
        public int PizzaId { get; set; }
        public string PizzaName { get; set; } = "";
        public string PizzaDescription { get; set; } = "";
        public string PizzaCategory { get; set; } = "";
        public decimal PizzaPrice { get; set; }
        public string? ImageUrl { get; set; }
    }

}
