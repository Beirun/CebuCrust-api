using CebuCrust_api.Config;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) => _db = db;

        public async Task<List<User>> GetAllAsync() =>
            await _db.Users.Include(u => u.Role).AsNoTracking().ToListAsync();

        public async Task<User?> GetByIdAsync(int id) =>
            await _db.Users.FindAsync(id);

        public async Task UpdateAsync(User u)
        {
            _db.Users.Update(u);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(User u)
        {
            u.DateDeleted = System.DateTime.UtcNow;
            _db.Users.Update(u);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}