using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddRoomRequestAfterMap : IMappingAction<AddRoomRequest, Room>
    {
        public void Process(AddRoomRequest source, Room destination, ResolutionContext context)
        {
            destination.RoomId = Guid.NewGuid();
        }
    }
}
