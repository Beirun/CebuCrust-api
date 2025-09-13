using CebuCrust_api.Config;
using CebuCrust_api.Models;
using CebuCrust_api.Interfaces;
using CebuCrust_api.ServiceModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CebuCrust_api.Services
{
    
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _db;
        public NotificationService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId) =>
            await _db.Notifications.Include(n => n.User)
                                   .AsNoTracking()
                                   .Where(n => n.UserId == userId)
                                   .ToListAsync();

        public async Task<Notification> CreateAsync(NotificationRequest request)
        {
            var n = new Notification
            {
                UserId = request.UserId,
                NotificationMessage = request.NotificationMessage,
                NotificationStatus = request.NotificationStatus,
                DateCreated = DateTime.UtcNow
            };

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
