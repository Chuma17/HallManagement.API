using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IChiefHallAdminRepository
    {
        Task<List<ChiefHallAdmin>> GetChiefHallAdmins();
        Task<ChiefHallAdmin> GetChiefHallAdmin(Guid chiefHallAdminId);
        Task<ChiefHallAdmin> GetChiefHallAdminByUserName(string userName);
        Task<bool> Exists(Guid chiefHallAdminId);
        Task<ChiefHallAdmin> UpdateChiefHallAdmin(Guid chiefHallAdminId, ChiefHallAdmin request);
        Task<ChiefHallAdmin> UpdateChiefHallAdminPasswordHash(Guid chiefHallAdminId, ChiefHallAdmin request);
        Task<ChiefHallAdmin> UpdateChiefHallAdminToken(string userName, ChiefHallAdmin request);
        Task<ChiefHallAdmin> DeleteChiefHallAdminAsync(Guid chiefHallAdminId);
        Task<ChiefHallAdmin> AddChiefHallAdminAsync(ChiefHallAdmin request);
        Task<bool> UpdateProfileImage(Guid chiefHallAdminId, string profileImageUrl);
    }
}
