using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddNotificationRequesAfterMap : IMappingAction<AddNotificationRequest, Notification>
    {
        public void Process(AddNotificationRequest source, Notification destination, ResolutionContext context)
        {
            destination.DateCreated = DateTime.Now;
            destination.NotiFicationId = Guid.NewGuid();
        }
    }
}
