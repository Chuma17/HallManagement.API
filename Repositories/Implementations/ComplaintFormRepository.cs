using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class ComplaintFormRepository : IComplaintForm
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

        public async Task<List<ComplaintForm>> GetComplaintFormsAsync()
        {
            var complaintForms = await _context.ComplaintForms.ToListAsync();
            return complaintForms;
        }
    }
}
