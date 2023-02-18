namespace HallManagementTest2.Requests.Add
{
    public class AddNotificationRequest
    {
        public string? NotificationContent { get; set; }
        public Guid HallId { get; set; }
    }
}
