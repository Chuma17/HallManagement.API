using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class ExitPassRepository : IExitPassRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentRepository _studentRepository;

        public ExitPassRepository(ApplicationDbContext context, IStudentRepository studentRepository)
        {
            _context = context;
            _studentRepository = studentRepository;
        }
        public async Task<ExitPass> AddExitPassAsync(ExitPass request, Guid hallId, Guid studentId)
        {
            var exitPass = await _context.ExitPasses.AddAsync(request);
            await _context.SaveChangesAsync();
            return exitPass.Entity;
        }

        public async Task<ExitPass> ApproveExitPass(Guid exitPassId)
        {
            var existingPass = await GetExitPass(exitPassId);
            if (existingPass != null)
            {
                existingPass.IsApproved = true;

                await _context.SaveChangesAsync();
                return existingPass;
            }
            return null;
        }

        public async Task<ExitPass> DeleteExitPass(Guid exitPassId)
        {
            var exitPass = await GetExitPass(exitPassId);

            if (exitPass != null)
            {
                _context.ExitPasses.Remove(exitPass);
                await _context.SaveChangesAsync();
                return exitPass;
            }

            return null;
        }

        public async Task<List<ExitPass>> GetApprovedExitPassesAsync(Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }

            List<ExitPass> approvedExitPasses = new List<ExitPass>();
            foreach (var exitPass in exitPasses)
            {
                if (exitPass.IsApproved)
                {
                    approvedExitPasses.Add(exitPass);
                }
            }
            return approvedExitPasses;
        }

        public async Task<ExitPass> GetExitPass(Guid exitPassId)
        {
            return await _context.ExitPasses.FirstOrDefaultAsync(x => x.ExitPassId == exitPassId);
        }

        public async Task<List<ExitPass>> GetExitPassesAsync()
        {
            var exitPasses = await _context.ExitPasses.ToListAsync();
            return exitPasses;
        }

        public async Task<List<ExitPass>> GetExitPassesForStudent(string matricNo, Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }

            List<ExitPass> studentExitPasses = new List<ExitPass>();
            foreach (var exitPass in exitPasses)
            {
                var student = await _studentRepository.GetStudentAsync(exitPass.StudentId);
                if (student.MatricNo == matricNo)
                {
                    studentExitPasses.Add(exitPass);
                }
            }
            return studentExitPasses;
        }

        public async Task<List<ExitPass>> GetExitPassesInHall(Guid hallId)
        {
            var exitPasses = await GetExitPassesAsync();
            List<ExitPass> hallExitPasses = new List<ExitPass>();
            foreach (var exitPass in exitPasses)
            {
                if (exitPass.HallId == hallId)
                {
                    hallExitPasses.Add(exitPass);
                }
            }
            return hallExitPasses;
        }

        public async Task<List<ExitPass>> GetPendingExitPassesAsync(Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }

            List<ExitPass> pendingExitPasses = new List<ExitPass>();
            foreach (var exitPass in exitPasses)
            {
                if (!exitPass.IsApproved && exitPass.DateOfExit < DateTime.Now)
                {
                    pendingExitPasses.Add(exitPass);
                }
            }
            return pendingExitPasses;
        }

        public async Task<List<ExitPass>> GetStudentsDueToReturn(Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }

            List<ExitPass> studentsDue = new List<ExitPass>();
            foreach (var exitPass in exitPasses)
            {
                if (exitPass.IsApproved && exitPass.DateOfReturn.Day == DateTime.Now.Day)
                {
                    studentsDue.Add(exitPass);
                }
            }
            return studentsDue;
        }

        public async Task<List<ExitPass>> GetStudentsOverDueToReturn(Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }

            List<ExitPass> studentsOverDue = new List<ExitPass>();
            foreach (var exitPass in exitPasses)
            {
                if (exitPass.IsApproved && DateTime.Now.Day > exitPass.DateOfReturn.Day && !exitPass.HasReturned)
                {
                    studentsOverDue.Add(exitPass);
                }
            }
            return studentsOverDue;
        }

        public async Task<ExitPass> UpdateStudentExitPass(Guid exitPassId)
        {
            var existingPass = await GetExitPass(exitPassId);
            if (existingPass != null)
            {
                existingPass.HasReturned = true;

                await _context.SaveChangesAsync();
                return existingPass;
            }
            return null;
        }
    }
}
