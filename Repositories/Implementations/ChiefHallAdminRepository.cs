using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class ChiefHallAdminRepository : IChiefHallAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public ChiefHallAdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ChiefHallAdmin> AddChiefHallAdminAsync(ChiefHallAdmin request)
        {
            var chiefHallAdmin = await _context.ChiefHallAdmins.AddAsync(request);
            await _context.SaveChangesAsync();
            return chiefHallAdmin.Entity;
        }

        public async Task<ChiefHallAdmin> DeleteChiefHallAdminAsync(Guid chiefHallAdminId)
        {
            var chiefHallAdmin = await GetChiefHallAdmin(chiefHallAdminId);

            if (chiefHallAdmin != null)
            {
                _context.ChiefHallAdmins.Remove(chiefHallAdmin);
                await _context.SaveChangesAsync();
                return chiefHallAdmin;
            }

            return null;
        }

        public async Task<bool> Exists(Guid chiefHallAdminId)
        {
            return await _context.ChiefHallAdmins.AnyAsync(x => x.ChiefHallAdminId == chiefHallAdminId);
        }

        public async Task<ChiefHallAdmin> GetChiefHallAdmin(Guid chiefHallAdminId)
        {
            var chiefHallAdmin = await _context.ChiefHallAdmins.FirstOrDefaultAsync(x => x.ChiefHallAdminId == chiefHallAdminId);
            return chiefHallAdmin;
        }

        public async Task<ChiefHallAdmin> GetChiefHallAdminByUserName(string userName)
        {
            var chiefHallAdmin = await _context.ChiefHallAdmins.FirstOrDefaultAsync(x => x.UserName == userName);
            if (chiefHallAdmin != null)
            {
                return chiefHallAdmin;
            }
            return null;
        }

        public async Task<List<ChiefHallAdmin>> GetChiefHallAdmins()
        {
            var chiefHallAdmins = await _context.ChiefHallAdmins.ToListAsync();

            return chiefHallAdmins;
        }

        public async Task<ChiefHallAdmin> UpdateChiefHallAdmin(Guid chiefHallAdminId, ChiefHallAdmin request)
        {
            var existingchiefHallAdmin = await GetChiefHallAdmin(chiefHallAdminId);
            if (existingchiefHallAdmin != null)
            {
                existingchiefHallAdmin.FirstName = request.FirstName;
                existingchiefHallAdmin.LastName = request.LastName;
                existingchiefHallAdmin.Email = request.Email;
                existingchiefHallAdmin.DateOfBirth = request.DateOfBirth;
                existingchiefHallAdmin.Gender = request.Gender;
                existingchiefHallAdmin.UserName = request.UserName;

                await _context.SaveChangesAsync();
                return existingchiefHallAdmin;
            }
            return null;
        }

        public async Task<ChiefHallAdmin> UpdateChiefHallAdminToken(string userName, ChiefHallAdmin request)
        {
            var existingchiefHallAdmin = await GetChiefHallAdminByUserName(userName);
            if (existingchiefHallAdmin != null)
            {
                existingchiefHallAdmin.AccessToken = request.AccessToken;
                existingchiefHallAdmin.RefreshToken = request.RefreshToken;

                await _context.SaveChangesAsync();
                return existingchiefHallAdmin;
            }
            return null;
        }

        public async Task<ChiefHallAdmin> UpdateChiefHallAdminPasswordHash(Guid chiefHallAdminId, ChiefHallAdmin request)
        {
            var existingchiefHallAdmin = await GetChiefHallAdmin(chiefHallAdminId);
            if (existingchiefHallAdmin != null)
            {              
                existingchiefHallAdmin.PasswordHash = request.PasswordHash;
                existingchiefHallAdmin.PasswordSalt = request.PasswordSalt;

                await _context.SaveChangesAsync();
                return existingchiefHallAdmin;
            }
            return null;
        }

        public async Task<bool> UpdateProfileImage(Guid chiefHallAdminId, string profileImageUrl)
        {
            throw new NotImplementedException();
        }
    }
}
