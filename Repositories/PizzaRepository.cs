using CebuCrust_api.Config;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Repositories
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly AppDbContext _db;
        public PizzaRepository(AppDbContext db) => _db = db;

        public async Task<List<Pizza>> GetAllAsync() =>
            await _db.Pizzas.AsNoTracking().ToListAsync();

        public async Task<Pizza?> GetByIdAsync(int id) =>
            await _db.Pizzas.AsNoTracking().FirstOrDefaultAsync(p => p.PizzaId == id);

        public async Task<Pizza> AddAsync(Pizza p)
        {
            _db.Pizzas.Add(p);
            await _db.SaveChangesAsync();
            return p;
        }

        public async Task<Pizza?> UpdateAsync(Pizza p)
        {
            _db.Pizzas.Update(p);
            await _db.SaveChangesAsync();
            return p;
        }

        public async Task<bool> DeleteAsync(Pizza p)
        {
            _db.Pizzas.Remove(p);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}