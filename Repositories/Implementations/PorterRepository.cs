using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HallManagementTest2.Repositories.Implementations
{
    public class PorterRepository : IPorterRepository
    {
        private readonly ApplicationDbContext _context;

        public PorterRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Porter> AddPorterAsync(Porter request)
        {
            var porter = await _context.Porters.AddAsync(request);
            await _context.SaveChangesAsync();
            return porter.Entity;
        }

        public async Task<Porter> DeletePorterAsync(Guid porterId)
        {
            var porter = await GetPorter(porterId);

            if (porter != null)
            {
                _context.Porters.Remove(porter);
                await _context.SaveChangesAsync();
                return porter;
            }

            return null;
        }

        public async Task<bool> Exists(Guid porterId)
        {
            return await _context.Porters.AnyAsync(x => x.PorterId == porterId);
        }

        public async Task<Porter> GetPorter(Guid porterId)
        {
            var porter = await _context.Porters.FirstOrDefaultAsync(x => x.PorterId == porterId);
            return porter;
        }

        public async Task<Porter> GetPorterByHall(Guid hallId)
        {
            var porter = await _context.Porters.FirstOrDefaultAsync(x => x.HallId == hallId);
            if (porter != null)
            {
                return porter;
            }
            return null;
        }

        public async Task<Porter> GetPorterByUserName(string userName)
        {
            var porter = await _context.Porters.FirstOrDefaultAsync(x => x.UserName == userName);
            if (porter != null)
            {
                return porter;
            }
            return null;
        }

        public async Task<List<Porter>> GetPorters()
        {
            var porter = await _context.Porters.ToListAsync();

            return porter;
        }

        public async Task<List<Porter>> GetPortersInHall(Guid hallId)
        {
            var porters = await GetPorters();
            var portersInHall = new List<Porter>();
            foreach (var porter in porters)
            {
                if (porter.HallId == hallId)
                {
                    portersInHall.Add(porter);
                }
            }
            return portersInHall;
        }

        public async Task<Porter> UpdatePorter(Guid porterId, Porter request)
        {
            var existingPorter = await GetPorter(porterId);
            if (existingPorter != null)
            {
                existingPorter.FirstName = request.FirstName;
                existingPorter.LastName = request.LastName;
                existingPorter.Email = request.Email;
                existingPorter.DateOfBirth = request.DateOfBirth;
                existingPorter.Mobile = request.Mobile;
                existingPorter.Gender = request.Gender;
                existingPorter.Address = request.Address;
                existingPorter.State = request.State;
                existingPorter.UserName = request.UserName;

                await _context.SaveChangesAsync();
                return existingPorter;
            }
            return null;
        }

        public async Task<Porter> UpdatePorterAccessToken(string userName, Porter request)
        {
            var existingPorter = await GetPorterByUserName(userName);
            if (existingPorter != null)
            {
                existingPorter.AccessToken = request.AccessToken;

                await _context.SaveChangesAsync();
                return existingPorter;
            }
            return null;
        }

        public async Task<Porter> UpdatePorterPasswordHash(Guid porterId, Porter request)
        {
            var existingPorter = await GetPorter(porterId);
            if (existingPorter != null)
            {                
                existingPorter.PasswordHash = request.PasswordHash;
                existingPorter.PasswordSalt = request.PasswordSalt;

                await _context.SaveChangesAsync();
                return existingPorter;
            }
            return null;
        }

        public async Task<bool> UpdateProfileImage(Guid porterId, string profileImageUrl)
        {
            throw new NotImplementedException();
        }
    }
}
