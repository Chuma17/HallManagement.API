using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IComplaintForm
    {
        Task<List<ComplaintForm>> GetComplaintFormsAsync();
        Task<ComplaintForm> AddComplaintFormAsync(ComplaintForm request);        
    }
}
