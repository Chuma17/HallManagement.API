using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Room> AddRoomAsync(Room request)
        {
            var room = await _context.Rooms.AddAsync(request);
            await _context.SaveChangesAsync();
            return room.Entity;
        }

        public async Task<Room> DeleteRoomAsync(Guid roomId)
        {
            var room = await GetRoomAsync(roomId);

            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return room;
            }

            return null;
        }

        public async Task<bool> Exists(Guid? roomId)
        {
            return await _context.Rooms.AnyAsync(x => x.RoomId == roomId);
        }

        public async Task<Room> GetRoomAsync(Guid? roomId)
        {
            return await _context.Rooms.Include(s => s.Students).FirstOrDefaultAsync(x => x.RoomId == roomId);
        }

        public async Task<Room> UpdateAvailableSpace(Guid? roomId, Room request)
        {
            var existingRoom = await GetRoomAsync(roomId);
            if (existingRoom != null)
            {
                existingRoom.AvailableSpace = request.AvailableSpace;
                existingRoom.IsFull = request.IsFull;

                await _context.SaveChangesAsync();
                return existingRoom;
            }
            return null;
        }        

        public async Task<Room> UpdateRoomCount(Guid roomId, Room request)
        {
            var existingRoom = await GetRoomAsync(roomId);
            if (existingRoom != null)
            {
                existingRoom.MaxOccupants = request.MaxOccupants;
                existingRoom.RoomGender = request.RoomGender;

                await _context.SaveChangesAsync();
                return existingRoom;
            }
            return null;
        }

        public async Task<Room> UpdateRoomStatus(Guid? roomId, Room request)
        {
            var existingRoom = await GetRoomAsync(roomId);
            if (existingRoom != null)
            {
                existingRoom.IsUnderMaintenance = request.IsUnderMaintenance;

                await _context.SaveChangesAsync();
                return existingRoom;
            }
            return null;
        }
    }
}
