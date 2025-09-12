using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Config;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using BCrypt.Net;

namespace CebuCrust_api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> UpdateAsync(int id, UserUpdateRequest request);
        Task<bool> DeleteAsync(int id);
        Task SaveImageAsync(int userId, IFormFile file);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public UserService(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _db.Users.Include(u => u.Role)
                           .AsNoTracking()
                           .ToListAsync();

        public async Task<User?> UpdateAsync(int id, UserUpdateRequest request)
        {
            var existing = await _db.Users.FindAsync(id);
            if (existing == null) return null;

            existing.UserFName = request.UserFName;
            existing.UserLName = request.UserLName;
            existing.UserEmail = request.UserEmail;
            existing.UserPhoneNo = request.UserPhoneNo;
            existing.DateUpdated = DateTime.UtcNow;

            // Handle password change
            if (!string.IsNullOrEmpty(request.CurrentPassword) ||
                !string.IsNullOrEmpty(request.NewPassword) ||
                !string.IsNullOrEmpty(request.ConfirmPassword))
            {
                if (string.IsNullOrEmpty(request.CurrentPassword) ||
                    string.IsNullOrEmpty(request.NewPassword) ||
                    string.IsNullOrEmpty(request.ConfirmPassword))
                    throw new Exception("All password fields are required for password change.");

                // Verify current password using BCrypt
                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, existing.PasswordHash))
                    throw new Exception("Current password is incorrect.");

                if (request.NewPassword != request.ConfirmPassword)
                    throw new Exception("New password and confirm password do not match.");

                existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            }

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Users.FindAsync(id);
            if (existing == null) return false;

            existing.DateDeleted = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task SaveImageAsync(int userId, IFormFile file)
        {
            if (file == null || file.Length == 0) return;

            var usersFolder = Path.Combine(_env.ContentRootPath, "Resources", "Users");
            if (!Directory.Exists(usersFolder)) Directory.CreateDirectory(usersFolder);

            var ext = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(usersFolder, userId + ext);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }

    public class UserUpdateRequest
    {
        public string? UserFName { get; set; }
        public string? UserLName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNo { get; set; }

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }

        public IFormFile? Image { get; set; }
    }
}
