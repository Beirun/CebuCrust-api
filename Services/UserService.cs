using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Config;
using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;
using CebuCrust_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using BCrypt.Net;

namespace CebuCrust_api.Services
{

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

        public async Task<UserResponse?> UpdateAsync(int id, UserUpdateRequest request)
        {
            var existing = await _db.Users.FindAsync(id);
            if (existing == null) return null;

            if(!string.IsNullOrEmpty(request.UserFName)) existing.UserFName = request.UserFName;
            if (!string.IsNullOrEmpty(request.UserLName)) existing.UserLName = request.UserLName;
            if (!string.IsNullOrEmpty(request.UserEmail)) existing.UserEmail = request.UserEmail;
            if (!string.IsNullOrEmpty(request.UserPhoneNo)) existing.UserPhoneNo = request.UserPhoneNo;
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
            return MapUser(existing);
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
