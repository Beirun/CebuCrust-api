using CebuCrust_api.Config;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CebuCrust_api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _db;
        public AccountRepository(AppDbContext db) => _db = db;

        public async Task<bool> EmailExistsAsync(string email) =>
            await _db.Users.AnyAsync(u => u.UserEmail == email);

        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _db.Users.Include(u => u.Role)
                           .FirstOrDefaultAsync(u => u.UserEmail == email);

        public async Task<bool> IsFirstUserAsync() =>
            !await _db.Users.AnyAsync();

        public async Task<Role?> GetRoleByNameAsync(string roleName) =>
            await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

        public async Task AddUserAsync(User u)
        {
            _db.Users.Add(u);
            await _db.SaveChangesAsync();
        }
    }
}