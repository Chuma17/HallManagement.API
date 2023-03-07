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

        public async Task<List<Room>> GetRoomsAsync()
        {
            var rooms = _context.Rooms.ToListAsync();
            return await rooms;
        }

        public async Task<List<Room>> GetRoomsInBlockAsync(Guid blockId)
        {
            var rooms = await GetRoomsAsync();
            var roomsInBlock = new List<Room>();
            foreach (var room in rooms)
            {
                if (room.BlockId == blockId)
                {
                    roomsInBlock.Add(room);
                }
            }
            return roomsInBlock;
        }

        public async Task<List<Room>> GetRoomsInHall(Guid hallId)
        {
            var rooms = await GetRoomsAsync();
            var roomsInHall = new List<Room>();
            foreach (var room in rooms)
            {
                if (room.HallId == hallId)
                {
                    roomsInHall.Add(room);
                }
            }
            return roomsInHall;
        }

        public async Task<Room> GetSingleRoomAsync(Guid roomId)
        {
            return await _context.Rooms.FirstOrDefaultAsync(x => x.RoomId == roomId);
        }

        public async Task<Room> UpdateAvailableSpace(Guid? roomId, Room request)
        {
            var existingRoom = await GetRoomAsync(roomId);
            if (existingRoom != null)
            {
                existingRoom.AvailableSpace = request.AvailableSpace;
                existingRoom.IsFull = request.IsFull;
                existingRoom.StudentCount = request.StudentCount;

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
