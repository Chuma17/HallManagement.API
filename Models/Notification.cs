namespace HallManagementTest2.Models
{
    public class Notification
    {
        public Guid NotiFicationId { get; set; }
        public DateTime DateCreated { get; set; }
        public string? NotificationContent { get; set; }

        public Guid HallId { get; set; }
    }
}
