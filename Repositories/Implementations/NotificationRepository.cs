using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Notification> CreateNotification(Notification request, Guid hallId)
        {
            var notification = await _context.Notifications.AddAsync(request);
            await _context.SaveChangesAsync();
            return notification.Entity;
        }

        public async Task<Notification> DeleteNotification(Guid notificationId)
        {
            var notification = await GetNotification(notificationId);

            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                return notification;
            }

            return null;
        }

        public async Task<List<Notification>> GetAllNotifications()
        {
            var notifications = _context.Notifications.ToListAsync();
            return await notifications;
        }

        public async Task<Notification> GetNotification(Guid notificationId)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x => x.NotiFicationId == notificationId);
        }

        public async Task<List<Notification>> GetNotificationInHall(Guid hallId)
        {
            var notifications = await GetAllNotifications();
            var notificationsInHall = new List<Notification>();
            foreach (var notification in notifications)
            {
                if (notification.HallId == hallId)
                {
                    notificationsInHall.Add(notification);
                }
            }
            return notificationsInHall;
        }

        public async Task<Notification> UpdateNotification(Guid notificationId, Notification request)
        {
            var existingNotification = await GetNotification(notificationId);
            if (existingNotification != null)
            {
                existingNotification.HallId = request.HallId;

                await _context.SaveChangesAsync();
                return existingNotification;
            }
            return null;
        }
    }
}
