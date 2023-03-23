using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class HallAdminRepository : IHallAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public HallAdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<HallAdmin> AddHallAdminAsync(HallAdmin request)
        {
            var hallAdmin = await _context.HallAdmins.AddAsync(request);
            await _context.SaveChangesAsync();
            return hallAdmin.Entity;
        }

        public async Task<HallAdmin> DeleteHallAdminAsync(Guid hallAdminId)
        {
            var hallAdmin = await GetHallAdmin(hallAdminId);

            if (hallAdmin != null)
            {
                _context.HallAdmins.Remove(hallAdmin);
                await _context.SaveChangesAsync();
                return hallAdmin;
            }

            return null;
        }

        public async Task<bool> Exists(Guid hallAdminId)
        {
            return await _context.HallAdmins.AnyAsync(x => x.HallAdminId == hallAdminId);
        }

        public async Task<HallAdmin> GetHallAdmin(Guid hallAdminId)
        {
            var hallAdmin = await _context.HallAdmins.FirstOrDefaultAsync(x => x.HallAdminId == hallAdminId);
            return hallAdmin;
        }

        public async Task<HallAdmin> GetHallAdminByHall(Guid hallId)
        {
            var hallAdmin = await _context.HallAdmins.FirstOrDefaultAsync(x => x.HallId == hallId);
            if (hallAdmin != null)
            {
                return hallAdmin;
            }
            return null;
        }

        public async Task<HallAdmin> GetHallAdminByUserName(string userName)
        {
            var hallAdmin = await _context.HallAdmins.FirstOrDefaultAsync(x => x.UserName == userName);
            if (hallAdmin != null)
            {
                return hallAdmin;
            }
            return null;
        }

        public async Task<List<HallAdmin>> GetHallAdminsByGender(string gender)
        {
            var hallAdmins = await _context.HallAdmins.ToListAsync();
            var filteredHallAdmins = new List<HallAdmin>();
            foreach (var hallAdmin in hallAdmins)
            {
                if (hallAdmin.Gender == gender)
                {
                    filteredHallAdmins.Add(hallAdmin);
                }
            }
            return filteredHallAdmins;
        }

        public async Task<HallAdmin> UpdateHallAdmin(Guid hallAdminId, HallAdmin request)
        {
            var existingHallAdmin = await GetHallAdmin(hallAdminId);
            if (existingHallAdmin != null)
            {
                existingHallAdmin.FirstName = request.FirstName;
                existingHallAdmin.LastName = request.LastName;
                existingHallAdmin.Email = request.Email;
                existingHallAdmin.Gender = request.Gender;
                existingHallAdmin.UserName = request.UserName;

                await _context.SaveChangesAsync();
                return existingHallAdmin;
            }
            return null;
        }

        public async Task<HallAdmin> UpdateHallAdminToken(string userName, HallAdmin request)
        {
            var existingHallAdmin = await GetHallAdminByUserName(userName);
            if (existingHallAdmin != null)
            {
                existingHallAdmin.AccessToken = request.AccessToken;
                existingHallAdmin.RefreshToken = request.RefreshToken;

                await _context.SaveChangesAsync();
                return existingHallAdmin;
            }
            return null;
        }

        public async Task<HallAdmin> UpdateHallAdminPasswordHash(Guid hallAdminId, HallAdmin request)
        {
            var existingHallAdmin = await GetHallAdmin(hallAdminId);
            if (existingHallAdmin != null)
            {                
                existingHallAdmin.PasswordHash = request.PasswordHash;
                existingHallAdmin.PasswordSalt = request.PasswordSalt;

                await _context.SaveChangesAsync();
                return existingHallAdmin;
            }
            return null;
        }

        public async Task<bool> UpdateProfileImage(Guid hallAdminId, string profileImageUrl)
        {
            throw new NotImplementedException();
        }
       
    }
}
