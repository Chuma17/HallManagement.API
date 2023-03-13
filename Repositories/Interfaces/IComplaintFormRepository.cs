using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IComplaintFormRepository
    {
        Task<List<ComplaintForm>> GetComplaintFormsAsync();
        Task<List<ComplaintForm>> GetComplaintFormsInHall(Guid hallId);
        Task<ComplaintForm> AddComplaintFormAsync(ComplaintForm request, Guid hallId);
        Task<ComplaintForm> DeleteComplaintForm(Guid complaintFormId);
        Task<ComplaintForm> GetComplaintForm(Guid complaintFormId);
        Task<ComplaintForm> UpdateComplaintForm(Guid complaintFormId, ComplaintForm request);
    }
}
