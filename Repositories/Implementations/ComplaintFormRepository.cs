using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class ComplaintFormRepository : IComplaintFormRepository
    {
        private readonly ApplicationDbContext _context;

        public ComplaintFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ComplaintForm> AddComplaintFormAsync(ComplaintForm request)
        {
            var complaintForm = await _context.ComplaintForms.AddAsync(request);
            await _context.SaveChangesAsync();
            return complaintForm.Entity;
        }

        public async Task<ComplaintForm> DeleteComplaintForm(Guid complaintFormId)
        {
            var complaintForm = await GetComplaintForm(complaintFormId);

            if (complaintForm != null)
            {
                _context.ComplaintForms.Remove(complaintForm);
                await _context.SaveChangesAsync();
                return complaintForm;
            }

            return null;
        }

        public async Task<ComplaintForm> GetComplaintForm(Guid complaintFormId)
        {
            return await _context.ComplaintForms.FirstOrDefaultAsync(x => x.ComplaintFormId == complaintFormId);
        }

        public async Task<List<ComplaintForm>> GetComplaintFormsAsync()
        {
            var complaintForms = await _context.ComplaintForms.ToListAsync();
            return complaintForms;
        }

        public async Task<List<ComplaintForm>> GetComplaintFormsInHall(Guid hallId)
        {
            var complaintForms = await GetComplaintFormsAsync();
            var complaintsInHall = new List<ComplaintForm>();
            foreach (var complaint in complaintForms)
            {
                if (complaint.HallId == hallId)
                {
                    complaintsInHall.Add(complaint);
                }
            }
            return complaintsInHall;
        }
    }
}
