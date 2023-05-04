using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IStudentDeviceRepository
    {
        Task<StudentDevice> AddStudentDeviceAsync(Guid hallId, StudentDevice request);
        Task<bool> Exists(Guid studentDeviceId);
        Task<StudentDevice> UpdateDeviceStatus(Guid studentDeviceId, StudentDevice request);
        Task<StudentDevice> DeleteStudentDeviceAsync(Guid studentDeviceId);
        Task<StudentDevice> GetStudentDeviceAsync(Guid studentDeviceId);
        Task<List<StudentDevice>> GetStudentDevices();
        Task<List<StudentDevice>> GetPendingDevices(Guid hallId);
        Task<List<StudentDevice>> GetStudentDevicesInHall(Guid hallId);
        Task<List<StudentDevice>> GetStudentDevicesForStudent(Guid studentId);
        Task<List<StudentDevice>> GetStudentDevicesByMatricNo(string matricNo);

    }
}
