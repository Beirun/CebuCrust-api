using CebuCrust_api.Authentication;
using CebuCrust_api.Models;
using CebuCrust_api.Repositories;
using CebuCrust_api.ServiceModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CebuCrust_api.Interfaces;
using System.Text.RegularExpressions;

namespace CebuCrust_api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IValidationService _vsvc;
        private readonly IWebHostEnvironment _env;
        private readonly TokenProvider _token;


        public AccountService(IAccountRepository repo, IConfiguration cfg, IWebHostEnvironment env, IValidationService vsvc)
        {
            _repo = repo;
            _vsvc = vsvc;
            _env = env;
            var opt = JwtSettings.Load(cfg);
            _token = new TokenProvider(opt);
        }

        public async Task<AuthResult> RegisterAsync(User u, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                throw new Exception("Passwords do not match");
            if(!await _vsvc.IsValidEmailAsync(u.UserEmail)) 
                throw new Exception("Email is invalid");

            if (await _repo.EmailExistsAsync(u.UserEmail))
                throw new Exception("Email already exists");

            if(!await _vsvc.IsValidPhoneAsync(u.UserPhoneNo!)) 
                throw new Exception("Phone number is invalid");
            if (password.Length < 8)
                    throw new Exception("Password must be atleast 8 characters long");
    
            bool isFirstUser = await _repo.IsFirstUserAsync();
            var roleName = isFirstUser ? "Admin" : "User";
            var role = await _repo.GetRoleByNameAsync(roleName)
                       ?? throw new Exception($"Role '{roleName}' not found");

            u.RoleId = role.RoleId;
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            u.DateCreated = DateTime.UtcNow;

            await _repo.AddUserAsync(u);

            return GenerateTokens(u);
        }

        public async Task<AuthResult?> LoginAsync(string email, string password)
        {
            var u = await _repo.GetUserByEmailAsync(email);
            if (u == null || !BCrypt.Net.BCrypt.Verify(password, u.PasswordHash))
                throw new Exception("Invalid Credentials");

            return GenerateTokens(u);
        }


        public async Task<AuthResult?> GoogleAsync(string email, string password)
        {
            var u = await _repo.GetUserByEmailAsync(email);
            if (u == null)
                throw new Exception("Set your phone number and your password");

            return GenerateTokens(u);
        }

        public async Task<string?> Refresh(string refreshToken)
        {
            var jwt = _token.Validate(refreshToken);
            if (jwt == null) return null;

            var u = await _repo.GetUserByIdAsync(int.Parse(jwt.Subject)) ?? throw new Exception("Invalid Session");
            return _token.CreateAccess(u);
        }


        private AuthResult GenerateTokens(User u)
        {
            var access = _token.CreateAccess(u);
            var refresh = _token.CreateRefresh(u.UserId);
            return new AuthResult
            {
                AccessToken = access,
                RefreshToken = refresh,
                User = MapUser(u)
            };
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
