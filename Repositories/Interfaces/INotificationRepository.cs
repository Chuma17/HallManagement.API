using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllNotifications();
        Task<Notification> CreateNotification(Notification request);
        Task<Notification> DeleteNotification(Guid notificationId);
        Task<Notification> GetNotification(Guid notificationId);
        Task<Notification> GetNotificationInHall(Guid hallId);
    }
}
