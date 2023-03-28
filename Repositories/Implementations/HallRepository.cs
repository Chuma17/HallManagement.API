using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HallManagementTest2.Repositories.Implementations
{
    public class HallRepository : IHallRepository
    {
        private readonly ApplicationDbContext _context;

        public HallRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Hall> AddHallAsync(Hall request)
        {
            var hall = await _context.Halls.AddAsync(request);
            await _context.SaveChangesAsync();
            return hall.Entity;
        }

        public async Task<Hall> DeleteHallAsync(Guid hallId)
        {
            var hall = await GetHallAsync(hallId);

            if (hall != null)
            {
                _context.Halls.Remove(hall);
                await _context.SaveChangesAsync();
                return hall;
            }

            return null;
        }

        public async Task<bool> Exists(Guid hallId)
        {
            return await _context.Halls.AnyAsync(x => x.HallId == hallId);
        }                        

        public async Task<Hall> GetHallAsync(Guid? hallId)
        {
            return await _context.Halls.FirstOrDefaultAsync(x => x.HallId == hallId);
        }

        public async Task<List<Hall>> GetHallsByGender(string gender)
        {
            var halls = await GetHallsAsync();
            var filteredHalls = new List<Hall>();
            foreach (var hall in halls)
            {
                if (hall.HallGender == gender)
                {
                    filteredHalls.Add(hall);
                }
            }
            return filteredHalls;
        }

        public async Task<List<Hall>> GetHallsAsync()
        {
            var hall = _context.Halls.ToListAsync();
            return await hall;
        }
        

        public async Task<Hall> GetPortersInHallAsync(Guid hallId)
        {
            return await _context.Halls.Include(s => s.Porters).FirstOrDefaultAsync(x => x.HallId == hallId);
        }
       

        public async Task<Hall> GetStudentDevicesInHallAsync(Guid hallId)
        {
            return await _context.Halls.Include(s => s.StudentDevices).FirstOrDefaultAsync(x => x.HallId == hallId);
        }

        public async Task<Hall> GetStudentsInHallAsync(Guid hallId)
        {
            return await _context.Halls.Include(s => s.Students).FirstOrDefaultAsync(x => x.HallId == hallId);
        }

        public async Task<List<Hall>> GetAssignedHalls(string gender, Guid hallTypeId)
        {
            var halls = await GetHallsByGender(gender);
            var filteredHalls = halls.Where(x => x.IsAssigned && x.HallTypeId == hallTypeId).ToList();    
            
            return filteredHalls;
        }

        public async Task<List<Hall>> GetAssignedHallsForPorters(string gender)
        {
            var halls = await GetHallsByGender(gender);
            var filteredHalls = halls.Where(x => x.IsAssigned).ToList();

            return filteredHalls;
        }

        public async Task<List<Hall>> GetUnassignedHalls(string gender)
        {
            var halls = await GetHallsByGender(gender);
            var filteredHalls = halls.Where(x => !x.IsAssigned).ToList();
            
            return filteredHalls;
        }

        public async Task<Hall> UpdateBlockCount(Guid hallId, Hall request)
        {
            var existingHall = await GetHallAsync(hallId);
            if (existingHall != null)
            {
                existingHall.BlockCount = request.BlockCount;

                await _context.SaveChangesAsync();
                return existingHall;
            }
            return null;
        }

        public async Task<Hall> UpdateHall(Guid hallId, Hall request)
        {
            var existingHall = await GetHallAsync(hallId);
            if (existingHall != null)
            {
                existingHall.RoomSpace= request.RoomSpace;
                existingHall.HallName = request.HallName;
                existingHall.HallGender = request.HallGender;

                await _context.SaveChangesAsync();
                return existingHall;
            }
            return null;
        }

        public async Task<Hall> UpdateHallStatus(Guid? hallId, Hall request)
        {
            var existingHall = await GetHallAsync(hallId);
            if (existingHall != null)
            {
                existingHall.IsAssigned = request.IsAssigned;               

                await _context.SaveChangesAsync();
                return existingHall;
            }
            return null;
        }

        public async Task<Hall> UpdateRoomCount(Guid hallId, Hall request)
        {
            var existingHall = await GetHallAsync(hallId);
            if (existingHall != null)
            {
                existingHall.RoomCount = request.RoomCount;
                existingHall.AvailableRooms = request.AvailableRooms;

                await _context.SaveChangesAsync();
                return existingHall;
            }
            return null;
        }

        public async Task<Hall> UpdateStudentCount(Guid? hallId, Hall request)
        {
            var existingHall = await GetHallAsync(hallId);
            if (existingHall != null)
            {
                existingHall.StudentCount = request.StudentCount;

                await _context.SaveChangesAsync();
                return existingHall;
            }
            return null;
        }        
    }
}
