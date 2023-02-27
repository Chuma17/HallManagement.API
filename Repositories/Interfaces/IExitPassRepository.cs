using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IExitPassRepository
    {
        Task<List<ExitPass>> GetExitPassesAsync();
        Task<List<ExitPass>> GetExitPassesInHall(Guid hallId);
        Task<List<ExitPass>> GetExitPassesForStudent(Guid studentId);
        Task<ExitPass> AddExitPassAsync(ExitPass request);
        Task<ExitPass> DeleteExitPass(Guid ExitPassId);
        Task<ExitPass> GetExitPass(Guid ExitPassId);
    }
}
