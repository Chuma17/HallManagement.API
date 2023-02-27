using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllNotifications();
        Task<Notification> CreateNotification(Notification request, Guid hallId);
        Task<Notification> DeleteNotification(Guid notificationId);
        Task<Notification> GetNotification(Guid notificationId);
        Task<Notification> UpdateNotification(Guid notificationId, Notification request);
        Task<List<Notification>> GetNotificationInHall(Guid hallId);
    }
}
