using CebuCrust_api.Config;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Repositories
{
    public class ResetRepository : IResetRepository
    {
        private readonly AppDbContext _db;
        public ResetRepository(AppDbContext db) => _db = db;

        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _db.Users.FirstOrDefaultAsync(u => u.UserEmail == email);

        public async Task AddResetAsync(Reset reset)
        {
            _db.Resets.Add(reset);
            await _db.SaveChangesAsync();
        }

        public async Task<Reset?> GetValidResetAsync(string resetCode) =>
            await _db.Resets
                     .Where(r => r.ResetCode == resetCode && !r.IsUsed)
                     .OrderByDescending(r => r.ResetId)
                     .FirstOrDefaultAsync();

        public async Task<User?> GetUserByIdAsync(int userId) =>
            await _db.Users.FindAsync(userId);

        public async Task UpdateAsync(params object[] entities)
        {
            _db.UpdateRange(entities);
            await _db.SaveChangesAsync();
        }
    }
}