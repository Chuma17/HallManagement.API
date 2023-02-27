using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IHallRepository
    {
        Task<List<Hall>> GetHallsAsync();
        Task<Hall> AddHallAsync(Hall request);
        Task<bool> Exists(Guid hallId);
        Task<Hall> UpdateHall(Guid hallId, Hall request);
        Task<Hall> UpdateBlockCount(Guid hallId, Hall request);
        Task<Hall> UpdateRoomCount(Guid hallId, Hall request);
        Task<Hall> UpdateStudentCount(Guid? hallId, Hall request);
        Task<Hall> UpdateHallStatus(Guid? hallId, Hall request);
        Task<Hall> GetHallAsync(Guid? hallId);
        Task<List<Hall>> GetHallsByGender(string gender);
        Task<List<Hall>> GetUnassignedHalls(string gender);
        Task<Hall> DeleteHallAsync(Guid hallId);
        Task<Hall> GetStudentsInHallAsync(Guid hallId);
        Task<Hall> GetPortersInHallAsync(Guid hallId);
        Task<Hall> GetRoomsInHallAsync(Guid hallId);
        Task<Hall> GetBlocksInHallAsync(Guid hallId);
        Task<Hall> GetStudentDevicesInHallAsync(Guid hallId);
        Task<Hall> GetComplaintFormsInHallAsync(Guid hallId);
        Task<Hall> GetNotificationInHallAsync(Guid hallId);
    }
}
