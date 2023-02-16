using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> AddRoomAsync(Room request);
        Task<bool> Exists(Guid? roomId);
        Task<Room> UpdateRoomStatus(Guid? roomId, Room request);
        Task<Room> UpdateAvailableSpace(Guid? roomId, Room request);
        Task<Room> UpdateRoomCount(Guid roomId, Room request);
        Task<Room> DeleteRoomAsync(Guid roomId);
        Task<Room> GetRoomAsync(Guid? roomId);
    }
}
