using CebuCrust_api.Interfaces;
using CebuCrust_api.Models;
using CebuCrust_api.Repositories;
using CebuCrust_api.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CebuCrust_api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        public NotificationService(INotificationRepository repo) => _repo = repo;

        public async Task<IEnumerable<NotificationResponse>> GetByUserAsync(int uid)
        {
            var notifs = await _repo.GetByUserAsync(uid);
            return notifs.Select(n => new NotificationResponse
            {
                NotificationId = n.NotificationId,
                NotificationMessage = n.NotificationMessage,
                NotificationStatus = n.NotificationStatus
            });
        }

        public async Task<NotificationResponse> CreateAsync(int uid, NotificationRequest request)
        {
            var n = new Notification
            {
                UserId = uid,
                NotificationMessage = request.NotificationMessage ?? "",
                NotificationStatus = request.NotificationStatus,
                DateCreated = DateTime.UtcNow
            };
            await _repo.AddNotificationAsync(n);

            return new NotificationResponse
            {
                NotificationId = n.NotificationId,
                NotificationMessage = n.NotificationMessage,
                NotificationStatus = n.NotificationStatus
            };
        }

        public async Task<NotificationResponse?> UpdateStatusAsync(int id, string status)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.NotificationStatus = status;
            existing.DateUpdated = DateTime.UtcNow;
            await _repo.UpdateNotificationAsync(existing);

            return new NotificationResponse
            {
                NotificationId = existing.NotificationId,
                NotificationMessage = existing.NotificationMessage,
                NotificationStatus = existing.NotificationStatus
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteNotificationAsync(existing);
            return true;
        }
    }
}
