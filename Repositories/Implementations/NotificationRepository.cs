using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;

namespace HallManagementTest2.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        public Task<Notification> CreateNotification(Notification request)
        {
            throw new NotImplementedException();
        }

        public Task<Notification> DeleteNotification(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetAllNotifications()
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetNotification(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetNotificationInHall(Guid hallId)
        {
            throw new NotImplementedException();
        }
    }
}
