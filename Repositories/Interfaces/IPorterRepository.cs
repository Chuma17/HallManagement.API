using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IPorterRepository
    {
        Task<List<Porter>> GetPorters();
        Task<Porter> GetPorter(Guid porterId);
        Task<Porter> GetPorterByUserName(string userName);
        Task<Porter> GetPorterByHall(Guid hallId);
        Task<bool> Exists(Guid porterId);
        Task<Porter> UpdatePorter(Guid porterId, Porter request);
        Task<Porter> UpdatePorterPasswordHash(Guid porterId, Porter request);
        Task<Porter> UpdatePorterAccessToken(string userName, Porter request);
        Task<Porter> DeletePorterAsync(Guid porterId);
        Task<Porter> AddPorterAsync(Porter request);
        Task<bool> UpdateProfileImage(Guid porterId, string profileImageUrl);
    }
}
