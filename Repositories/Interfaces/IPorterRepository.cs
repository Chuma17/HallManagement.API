using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IPorterRepository
    {
        Task<List<Porter>> GetPorters();
        Task<List<Porter>> GetPortersByGender(string gender);
        Task<List<Porter>> GetPortersInHall(Guid hallId);
        Task<Porter> GetPorter(Guid porterId);
        Task<Porter> GetPorterByUserName(string userName);
        Task<bool> Exists(Guid porterId);
        Task<Porter> UpdatePorter(Guid porterId, Porter request);
        Task<Porter> UpdatePorterPasswordHash(Guid porterId, Porter request);
        Task<Porter> UpdatePorterToken(string userName, Porter request);
        Task<Porter> DeletePorterAsync(Guid porterId);
        Task<Porter> AddPorterAsync(Porter request);
        Task<bool> UpdateProfileImage(Guid porterId, string profileImageUrl);
    }
}
