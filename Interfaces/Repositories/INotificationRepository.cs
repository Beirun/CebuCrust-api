using CebuCrust_api.Models;
namespace CebuCrust_api.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetByUserAsync(int uid);
        Task<Notification?> GetByIdAsync(int id);
        Task AddNotificationAsync(Notification n);
        Task UpdateNotificationAsync(Notification n);
        Task DeleteNotificationAsync(Notification n);
    }
}