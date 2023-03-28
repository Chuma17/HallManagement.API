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
        public async Task<ComplaintForm> AddComplaintFormAsync(ComplaintForm request, Guid hallId)
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

        public async Task<List<ComplaintForm>> GetComplaintFormsInBlock(Guid blockId)
        {
            var complaints = await GetComplaintFormsAsync();
            var complaintForms = complaints.Where(complaint => complaint.BlockId == blockId).ToList();

            complaintForms = complaintForms.OrderBy(complaint => complaint.DateCreated).ToList();

            return complaintForms;
        }

        public async Task<List<ComplaintForm>> GetComplaintFormsInHall(Guid hallId)
        {
            var complaints = await GetComplaintFormsAsync();
            var complaintForms = complaints.Where(complaint => complaint.HallId == hallId).ToList();

            complaintForms = complaintForms.OrderBy(complaint => complaint.DateCreated).ToList();
            
            return complaintForms;
        }

        public async Task<List<ComplaintForm>> GetComplaintFormsInRoom(Guid roomId)
        {
            var complaints = await GetComplaintFormsAsync();
            var complaintForms = complaints.Where(complaint => complaint.RoomId == roomId).ToList();

            complaintForms = complaintForms.OrderBy(complaint => complaint.DateCreated).ToList();

            return complaintForms;
        }

        public async Task<ComplaintForm> UpdateComplaintForm(Guid complaintFormId, ComplaintForm request)
        {
            var existingComplaint = await GetComplaintForm(complaintFormId);
            if (existingComplaint != null)
            {
                existingComplaint.HallId = request.HallId;

                await _context.SaveChangesAsync();
                return existingComplaint;
            }
            return null;
        }
    }
}
