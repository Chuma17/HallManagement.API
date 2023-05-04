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
                existingPass.Status = "Approved";

                await _context.SaveChangesAsync();
                return existingPass;
            }
            return null;
        }

        public async Task<ExitPass> DeclineExitPass(Guid exitPassId)
        {
            var existingPass = await GetExitPass(exitPassId);
            if (existingPass != null)
            {
                existingPass.Status = "Declined";

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
            var approved = exitPasses.Where(exitPass => exitPass.Status == "Approved").ToList();
            approved = approved.OrderBy(exitPass => exitPass.DateIssued).ToList();
            
            return approved;
        }

        public async Task<List<ExitPass>> GetDeclinedExitPassesAsync(Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }
            var declined = exitPasses.Where(exitPass => exitPass.Status == "Declined").ToList();
            declined = declined.OrderBy(exitPass => exitPass.DateIssued).ToList();

            return declined;
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
            var exitPass = await GetExitPassesAsync();
            var exitPasses = exitPass.Where(exitPass => exitPass.HallId == hallId).ToList();

            exitPasses = exitPasses.OrderBy(exitPass => exitPass.DateIssued).ToList();
            return exitPasses;
        }

        public async Task<List<ExitPass>> GetPendingExitPassesAsync(Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }
            var pending = exitPasses.Where(exitPass => exitPass.Status == "Pending" && exitPass.DateOfExit < DateTime.Now).ToList();
            pending = pending.OrderBy(exitPass => exitPass.DateIssued).ToList();

            return pending;                        
        }

        public async Task<List<ExitPass>> GetStudentsDueToReturn(Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }

            var studentsDue = exitPasses.Where(exitPass => exitPass.DateOfReturn.Day == DateTime.Now.Day && exitPass.Status == "Approved").ToList();
            studentsDue = studentsDue.OrderBy(students => students.DateIssued).ToList();
         
            return studentsDue;
        }

        public async Task<List<ExitPass>> GetStudentsOverDueToReturn(Guid hallId)
        {
            var exitPasses = await GetExitPassesInHall(hallId);
            if (exitPasses == null)
            {
                return null;
            }

            var studentsOverDue = exitPasses.Where(exitPass => exitPass.Status == "Approved" && DateTime.Now.Day > exitPass.DateOfReturn.Day && !exitPass.HasReturned).ToList();
            studentsOverDue = studentsOverDue.OrderBy(students => students.DateIssued).ToList();
            
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
