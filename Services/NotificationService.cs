// Services/NotificationService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CebuCrust_api.Config;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;

namespace CebuCrust_api.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<Notification> CreateAsync(Notification n);
        Task<Notification?> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
    }

    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _db;
        public NotificationService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId) =>
            await _db.Notifications.Include(n => n.User)
                                   .AsNoTracking()
                                   .Where(n => n.UserId == userId)
                                   .ToListAsync();

        public async Task<Notification> CreateAsync(Notification n)
        {
            n.DateCreated = DateTime.UtcNow;
            _db.Notifications.Add(n);
            await _db.SaveChangesAsync();
            return n;
        }

        public async Task<Notification?> UpdateStatusAsync(int id, string status)
        {
            var existing = await _db.Notifications.FindAsync(id);
            if (existing == null) return null;

            existing.NotificationStatus = status;
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Notifications.FindAsync(id);
            if (existing == null) return false;
            _db.Notifications.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
