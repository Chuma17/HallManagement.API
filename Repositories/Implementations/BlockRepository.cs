using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class BlockRepository : IBlockRepository
    {
        private readonly ApplicationDbContext _context;

        public BlockRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Block> AddBlockAsync(Block request)
        {
            var block = await _context.Blocks.AddAsync(request);
            await _context.SaveChangesAsync();
            return block.Entity;
        }

        public async Task<Block> DeleteBlockAsync(Guid blockId)
        {
            var block = await GetBlockAsync(blockId);

            if (block != null)
            {
                _context.Blocks.Remove(block);
                await _context.SaveChangesAsync();
                return block;
            }

            return null;
        }

        public async Task<bool> Exists(Guid? blockId)
        {
            return await _context.Blocks.AnyAsync(x => x.BlockId == blockId);
        }

        public async Task<Block> GetBlockAsync(Guid? blockId)
        {
            return await _context.Blocks.Include(s => s.Rooms).FirstOrDefaultAsync(x => x.BlockId == blockId);
        }

        public async Task<List<Block>> GetBlocksAsync()
        {
            var blocks = _context.Blocks.ToListAsync();
            return await blocks;
        }

        public async Task<List<Block>> GetBlocksInHall(Guid hallId)
        {
            var blocks = await GetBlocksAsync();
            var blocksInHall = new List<Block>();
            foreach (var block in blocks)
            {
                if (block.HallId == hallId)
                {
                    blocksInHall.Add(block);
                }
            }
            return blocksInHall;
        }

        public async Task<Block> UpdateBlock(Guid blockId, Block request)
        {
            throw new NotImplementedException();
        }

        public async Task<Block> UpdateBlockRoomCount(Guid blockId, Block request)
        {
            var existingBlock = await GetBlockAsync(blockId);
            if (existingBlock != null)
            {
                existingBlock.BlockGender = request.BlockGender;
                existingBlock.RoomSpace = request.RoomSpace;
                existingBlock.AvailableRooms = request.AvailableRooms;
                existingBlock.RoomCount= request.RoomCount;
                existingBlock.StudentCount = request.StudentCount;

                await _context.SaveChangesAsync();
                return existingBlock;
            }
            return null;
        }        
    }
}
