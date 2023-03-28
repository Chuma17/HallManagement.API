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
        Task<List<Hall>> GetAssignedHallsForPorters(string gender);
        Task<List<Hall>> GetAssignedHalls(string gender, Guid hallTypeId);
        Task<Hall> DeleteHallAsync(Guid hallId);
        Task<Hall> GetStudentsInHallAsync(Guid hallId);
        Task<Hall> GetPortersInHallAsync(Guid hallId);       
        Task<Hall> GetStudentDevicesInHallAsync(Guid hallId);
    }
}
