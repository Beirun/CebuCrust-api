using CebuCrust_api.Config;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _db;
        public NotificationRepository(AppDbContext db) => _db = db;

        public async Task<List<Notification>> GetByUserAsync(int uid) =>
            await _db.Notifications.AsNoTracking()
                                   .Where(n => n.UserId == uid)
                                   .ToListAsync();

        public async Task<Notification?> GetByIdAsync(int id) =>
            await _db.Notifications.FindAsync(id);

        public async Task AddNotificationAsync(Notification n)
        {
            _db.Notifications.Add(n);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateNotificationAsync(Notification n)
        {
            _db.Notifications.Update(n);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Notification n)
        {
            _db.Notifications.Remove(n);
            await _db.SaveChangesAsync();
        }
    }
}
