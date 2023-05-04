using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IExitPassRepository
    {
        Task<List<ExitPass>> GetExitPassesAsync();
        Task<List<ExitPass>> GetPendingExitPassesAsync(Guid hallId);
        Task<List<ExitPass>> GetDeclinedExitPassesAsync(Guid hallId);
        Task<List<ExitPass>> GetApprovedExitPassesAsync(Guid hallId);
        Task<List<ExitPass>> GetStudentsDueToReturn(Guid hallId);
        Task<List<ExitPass>> GetStudentsOverDueToReturn(Guid hallId);
        Task<List<ExitPass>> GetExitPassesInHall(Guid hallId);
        Task<List<ExitPass>> GetExitPassesForStudent(string matricNo, Guid hallId);
        Task<ExitPass> AddExitPassAsync(ExitPass request, Guid hallId, Guid studentId);
        Task<ExitPass> DeleteExitPass(Guid exitPassId);
        Task<ExitPass> GetExitPass(Guid exitPassId);
        Task<ExitPass> ApproveExitPass(Guid exitPassId);
        Task<ExitPass> DeclineExitPass(Guid exitPassId);
        Task<ExitPass> UpdateStudentExitPass(Guid exitPassId);
    }
}
