using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CebuCrust_api.Config;
using CebuCrust_api.Models;
using CebuCrust_api.Interfaces;
using CebuCrust_api.ServiceModels;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace CebuCrust_api.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;
        private readonly IWebHostEnvironment _env;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AccountService(AppDbContext db, IConfiguration cfg, IWebHostEnvironment env)
        {
            _db = db;
            _cfg = cfg;
            _env = env;
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
            if (u == null) throw new Exception("Invalid Credentials");
            if (!BCrypt.Net.BCrypt.Verify(password, u.PasswordHash)) throw new Exception("Incorrect Password");

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
            return new AuthResult
            {
                AccessToken = access,
                RefreshToken = refresh,
                User = MapUser(u)
            };
        }

        private string CreateAccessToken(int userId, IEnumerable<Claim> extraClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtKey));
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
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtKey));
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

        private UserResponse MapUser(User u)
        {
            byte[]? imgData = null;
            var folder = Path.Combine(_env.ContentRootPath, "Resources", "Users");
            if (Directory.Exists(folder))
            {
                var file = Directory.GetFiles(folder, u.UserId + ".*").FirstOrDefault();
                if (file != null)
                    imgData = File.ReadAllBytes(file);
                else
                {
                    var def = Path.Combine(folder, "default.png");
                    if (File.Exists(def))
                        imgData = File.ReadAllBytes(def);
                }
            }

            return new UserResponse
            {
                UserId = u.UserId,
                FirstName = u.UserFName,
                LastName = u.UserLName,
                Email = u.UserEmail,
                PhoneNo = u.UserPhoneNo,
                DateCreated = u.DateCreated,
                ProfileImage = imgData
            };
        }
    }
}
