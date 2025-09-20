using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CebuCrust_api.Config;
using CebuCrust_api.ServiceModels;
using CebuCrust_api.Models;
using CebuCrust_api.Interfaces;

namespace CebuCrust_api.Services
{
    public class ResetService: IResetService
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService;

        public ResetService(AppDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        // Request a password reset
        public async Task<bool> ResetRequestAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
            if (user == null) throw new Exception("Email not found");

            byte[] bytes = RandomNumberGenerator.GetBytes(64);
            string resetCode = Convert.ToHexString(bytes).ToLower();

            // Store reset code in database
            var reset = new Reset
            {
                UserId = user.UserId,
                ResetCode = resetCode,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };
            _db.Resets.Add(reset);
            await _db.SaveChangesAsync();

            // Send email
            await _emailService.SendResetEmailAsync(email, resetCode);

            return true;
        }

        // Verify if a reset code is valid
        public async Task<bool> ResetVerifyAsync(string resetCode)
        {
            var reset = await _db.Resets
                .Where(r => r.ResetCode == resetCode && !r.IsUsed)
                .OrderByDescending(r => r.ResetId)
                .FirstOrDefaultAsync();

            if (reset == null || reset.ExpiresAt < DateTime.UtcNow)
                return false;

            return true;
        }

        // Reset the password using the code
        public async Task<bool> ResetPasswordAsync(string resetCode, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
                throw new Exception("Passwords do not match");

            var reset = await _db.Resets
                .Where(r => r.ResetCode == resetCode && !r.IsUsed)
                .OrderByDescending(r => r.ResetId)
                .FirstOrDefaultAsync();

            if (reset == null || reset.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired reset code");

            var user = await _db.Users.FindAsync(reset.UserId);
            if (user == null) throw new Exception("User not found");

            // Update password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Mark reset code as used
            reset.IsUsed = true;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
