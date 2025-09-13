using CebuCrust_api.Models;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<Notification> CreateAsync(NotificationRequest request);
        Task<Notification?> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
    }

}
