using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;

namespace HallManagementTest2.Repositories.Implementations
{
    public class ExitPassRepository : IExitPassRepository
    {
        public Task<ExitPass> AddExitPassAsync(ExitPass request)
        {
            throw new NotImplementedException();
        }

        public Task<ExitPass> DeleteExitPass(Guid ExitPassId)
        {
            throw new NotImplementedException();
        }

        public Task<ExitPass> GetExitPass(Guid ExitPassId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExitPass>> GetExitPassesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ExitPass>> GetExitPassesForStudent(Guid studentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExitPass>> GetExitPassesInHall(Guid hallId)
        {
            throw new NotImplementedException();
        }
    }
}
