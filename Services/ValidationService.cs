using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CebuCrust_api.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CebuCrust_api.Services
{
    public class ValidationService : IValidationService
    {
        readonly Regex emailR = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        readonly Regex phoneR = new Regex(
            @"^\+?\d{7,15}$",
            RegexOptions.Compiled
        );

        readonly Regex imageExtR = new Regex(
            @"\.(jpg|jpeg|png|gif|bmp|webp)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        const long MaxImageBytes = 5L * 1024L * 1024L;

        public ValidationService() {}

        public Task<bool> IsValidEmailAsync(string e)
        {
            if (string.IsNullOrWhiteSpace(e)) return Task.FromResult(false);
            return Task.FromResult(emailR.IsMatch(e));
        }

        public Task<bool> IsValidPhoneAsync(string p)
        {
            if (string.IsNullOrWhiteSpace(p)) return Task.FromResult(false);
            return Task.FromResult(phoneR.IsMatch(p));
        }

        public Task<bool> IsValidImageAsync(IFormFile f)
        {
            if (f == null) return Task.FromResult(false);
            if (f.Length == 0 || f.Length > MaxImageBytes) return Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(f.FileName)) return Task.FromResult(false);
            return Task.FromResult(imageExtR.IsMatch(f.FileName));
        }
    }
}
