using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CebuCrust_api.Repositories;
using CebuCrust_api.ServiceModels;
using CebuCrust_api.Models;
using CebuCrust_api.Interfaces;

namespace CebuCrust_api.Services
{
    public class ResetService : IResetService
    {
        private readonly IResetRepository _repo;
        private readonly IEmailService _emailService;

        public ResetService(IResetRepository repo, IEmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }

        public async Task<bool> ResetRequestAsync(string email)
        {
            var user = await _repo.GetUserByEmailAsync(email);
            if (user == null) throw new Exception("Email not found");

            byte[] bytes = RandomNumberGenerator.GetBytes(64);
            string resetCode = Convert.ToHexString(bytes).ToLower();

            var reset = new Reset
            {
                UserId = user.UserId,
                ResetCode = resetCode,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };

            await _repo.AddResetAsync(reset);
            await _emailService.SendResetEmailAsync(email, resetCode);

            return true;
        }

        public async Task<bool> ResetVerifyAsync(string resetCode)
        {
            var reset = await _repo.GetValidResetAsync(resetCode);
            return reset != null && reset.ExpiresAt >= DateTime.UtcNow;
        }

        public async Task<bool> ResetPasswordAsync(string resetCode, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
                throw new Exception("Passwords do not match");

            var reset = await _repo.GetValidResetAsync(resetCode);
            if (reset == null || reset.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired reset code");

            var user = await _repo.GetUserByIdAsync(reset.UserId);
            if (user == null) throw new Exception("User not found");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            reset.IsUsed = true;

            await _repo.UpdateAsync(user, reset);
            return true;
        }
    }
}
