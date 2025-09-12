using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CebuCrust_api.Data;
using CebuCrust_api.Models;

namespace CebuCrust_api.Services
{
    public record AuthResult(string Token, UserResponse User);

    public record UserResponse(int UserId, string FirstName, string LastName, string Email,
                          string? PhoneNo, DateTime DateCreated);

    public interface IAccountService
    {
        Task<AuthResult> RegisterAsync(User u, string password);
        Task<AuthResult?> LoginAsync(string email, string password);
    }

    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;

        public AccountService(AppDbContext db, IConfiguration cfg)
        {
            _db = db;
            _cfg = cfg;
        }

        public async Task<AuthResult> RegisterAsync(User u, string password)
        {
            if (await _db.Users.AnyAsync(x => x.UserEmail == u.UserEmail))
                throw new Exception("Email already exists");


            bool isFirstUser = !await _db.Users.AnyAsync();
            var roleName = isFirstUser ? "Admin" : "User";
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null) throw new Exception($"Role '{roleName}' not found in database");

            u.RoleId = role.RoleId;
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            u.DateCreated = DateTime.UtcNow;
            _db.Users.Add(u);
            await _db.SaveChangesAsync();
            return new AuthResult(GenerateJwt(u), MapUser(u));
        }

        public async Task<AuthResult?> LoginAsync(string email, string password)
        {
            var u = await _db.Users.Include(r => r.Role)
                                   .FirstOrDefaultAsync(x => x.UserEmail == email);
            if (u == null || !BCrypt.Net.BCrypt.Verify(password, u.PasswordHash))
                return null;
            return new AuthResult(GenerateJwt(u), MapUser(u));
        }

        private string GenerateJwt(User u)
        {
            Console.WriteLine(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, u.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, u.UserEmail),
                new Claim(ClaimTypes.Role, u.Role?.RoleName ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:ExpireMinutes"] ?? "60")),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static UserResponse MapUser(User u) =>
            new UserResponse(u.UserId, u.UserFName, u.UserLName, u.UserEmail,
                        u.UserPhoneNo, u.DateCreated);
    }
}
