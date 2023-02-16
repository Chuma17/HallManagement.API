using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(Guid studentId);
        Task<Student> GetStudentDevicesAsync(Guid studentId);
        Task<Student> GetStudentByUserName(string userName);
        Task<bool> Exists(Guid studentId);
        Task<Student> UpdateStudent(Guid studentId, Student request);
        Task<Student> UpdateStudentPasswordHash(Guid studentId, Student request);
        Task<Student> UpdateStudentAccessToken(string userName, Student request);
        Task<Student> DeleteStudentAsync(Guid studentId);
        Task<Student> AddStudentAsync(Student request);
        Task<Student> JoinHall(Guid? hallId, Guid studentId);
        Task<Student> JoinBlock(Guid? blockId, Guid studentId);
        Task<Student> JoinRoom(Guid? roomId, Guid studentId);        
        Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl);

    }
}
