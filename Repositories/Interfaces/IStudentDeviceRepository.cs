using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IStudentDeviceRepository
    {
        Task<StudentDevice> AddStudentDeviceAsync(StudentDevice request);
        Task<bool> Exists(Guid studentDeviceId);
        Task<StudentDevice> UpdateStudentDevice(Guid studentDeviceId, StudentDevice request);
        Task<StudentDevice> DeleteStudentDeviceAsync(Guid studentDeviceId);
        Task<StudentDevice> GetStudentDeviceAsync(Guid studentDeviceId);
        Task<List<StudentDevice>> GetStudentsByMatricNo(Guid hallId, string matricNo);

    }
}
