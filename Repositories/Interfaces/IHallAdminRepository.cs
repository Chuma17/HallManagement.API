using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IHallAdminRepository
    {
        Task<List<HallAdmin>> GetHallAdminsByGender(string gender);
        Task<HallAdmin> GetHallAdmin(Guid hallAdminId);
        Task<HallAdmin> GetHallAdminByUserName(string userName);
        Task<HallAdmin> GetHallAdminByHall(Guid hallId);
        Task<bool> Exists(Guid hallAdminId);
        Task<HallAdmin> UpdateHallAdmin(Guid hallAdminId, HallAdmin request);
        Task<HallAdmin> UpdateHallAdminPasswordHash(Guid hallAdminId, HallAdmin request);
        Task<HallAdmin> UpdateHallAdminToken(string userName, HallAdmin request);
        Task<HallAdmin> DeleteHallAdminAsync(Guid hallAdminId);
        Task<HallAdmin> AddHallAdminAsync(HallAdmin request);
        Task<bool> UpdateProfileImage(Guid hallAdminId, string profileImageUrl);
    }
}
