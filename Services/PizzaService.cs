// Services/PizzaService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Config;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace CebuCrust_api.Services
{

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
                PizzaDescription = p.PizzaDescription,
                PizzaCategory = p.PizzaCategory,
                PizzaPrice = p.PizzaPrice,
                pizzaImage = imgData
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

}
