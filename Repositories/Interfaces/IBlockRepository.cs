using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IBlockRepository
    {
        Task<Block> AddBlockAsync(Block request);
        Task<bool> Exists(Guid? blockId);
        Task<Block> UpdateBlock(Guid blockId, Block request);
        Task<Block> UpdateBlockRoomCount(Guid blockId, Block request);
        Task<Block> DeleteBlockAsync(Guid blockId);
        Task<Block> GetBlockAsync(Guid? blockId);
    }
}
