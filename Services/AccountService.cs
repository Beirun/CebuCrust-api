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
    public record AuthResult(string AccessToken, string RefreshToken, UserResponse User);
    public record UserResponse(int UserId, string FirstName, string LastName, string Email,
                      string? PhoneNo, DateTime DateCreated);
    public interface IAccountService
    {
        Task<AuthResult> RegisterAsync(User u, string password, string confirmPassword);
        Task<AuthResult?> LoginAsync(string email, string password);
        string? Refresh(string refreshToken);
    }

    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AccountService(AppDbContext db, IConfiguration cfg)
        {
            _db = db;
            _cfg = cfg;
            _jwtKey = _cfg["Jwt:Key"]!;
            _issuer = _cfg["Jwt:Issuer"]!;
            _audience = _cfg["Jwt:Audience"]!;
        }

        public async Task<AuthResult> RegisterAsync(User u, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                throw new Exception("Passwords do not match");

            if (await _db.Users.AnyAsync(x => x.UserEmail == u.UserEmail))
                throw new Exception("Email already exists");

            bool isFirstUser = !await _db.Users.AnyAsync();
            var roleName = isFirstUser ? "Admin" : "User";
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName)
                       ?? throw new Exception($"Role '{roleName}' not found");

            u.RoleId = role.RoleId;
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            u.DateCreated = DateTime.UtcNow;
            _db.Users.Add(u);
            await _db.SaveChangesAsync();

            return GenerateTokens(u);
        }


        public async Task<AuthResult?> LoginAsync(string email, string password)
        {
            var u = await _db.Users.Include(r => r.Role)
                                   .FirstOrDefaultAsync(x => x.UserEmail == email);
            if (u == null || !BCrypt.Net.BCrypt.Verify(password, u.PasswordHash))
                return null;

            return GenerateTokens(u);
        }

        public string? Refresh(string refreshToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
                handler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    ClockSkew = TimeSpan.Zero
                }, out var validated);

                var jwt = (JwtSecurityToken)validated;
                var uid = jwt.Subject;
                return CreateAccessToken(int.Parse(uid), jwt.Claims);
            }
            catch { return null; }
        }

        private AuthResult GenerateTokens(User u)
        {
            var access = CreateAccessToken(u.UserId, new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, u.UserEmail),
                new Claim(ClaimTypes.Role, u.Role?.RoleName ?? "User")
            });

            var refresh = CreateRefreshToken(u.UserId);
            return new AuthResult(access, refresh, MapUser(u));
        }

        private string CreateAccessToken(int userId, IEnumerable<Claim> extraClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()) };
            claims.AddRange(extraClaims);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:ExpireMinutes"] ?? "60")),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string CreateRefreshToken(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("typ","refresh")
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static UserResponse MapUser(User u) =>
            new UserResponse(u.UserId, u.UserFName, u.UserLName, u.UserEmail,
                        u.UserPhoneNo, u.DateCreated);
    }
    public class RegisterRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNo { get; set; }
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }


    public class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
