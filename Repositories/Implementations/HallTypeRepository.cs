﻿using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class HallTypeRepository : IHallTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public HallTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<HallType> AddHallTypeAsync(HallType request)
        {
            var hallType = await _context.HallTypes.AddAsync(request);
            await _context.SaveChangesAsync();
            return hallType.Entity;
        }

        public async Task<HallType> DeleteHallTypeAsync(Guid hallTypeId)
        {
            var hallType = await GetHallTypeAsync(hallTypeId);

            if (hallType != null)
            {
                _context.HallTypes.Remove(hallType);
                await _context.SaveChangesAsync();
                return hallType;
            }

            return null;
        }

        public async Task<bool> Exists(Guid hallTypeId)
        {
            return await _context.HallTypes.AnyAsync(x => x.HallTypeId == hallTypeId);
        }

        public async Task<HallType> GetHallTypeAsync(Guid hallTypeId)
        {
            return await _context.HallTypes.Include(s => s.Halls).FirstOrDefaultAsync(x => x.HallTypeId == hallTypeId);
        }

        public async Task<HallType> GetHallTypeForHall(Guid hallTypeId)
        {
            var hallType = await _context.HallTypes.FirstOrDefaultAsync(x => x.HallTypeId == hallTypeId);
            return hallType;
        }

        public async Task<List<HallType>> GetHallTypesAsync()
        {
            var hallType = _context.HallTypes.ToListAsync();
            return await hallType;
        }

        public async Task<HallType> UpdateHallType(Guid hallTypeId, HallType request)
        {
            var existingHallType = await GetHallTypeAsync(hallTypeId);
            if (existingHallType != null)
            {
                existingHallType.Description = request.Description;
                existingHallType.RoomSpaceCount = request.RoomSpaceCount;

                await _context.SaveChangesAsync();
                return existingHallType;
            }
            return null;
        }
    }
}
