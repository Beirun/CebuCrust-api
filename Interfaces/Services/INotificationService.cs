using CebuCrust_api.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CebuCrust_api.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponse>> GetByUserAsync(int userId);
        Task<NotificationResponse> CreateAsync(int userId, NotificationRequest request);
        Task<NotificationResponse?> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
    }
}
