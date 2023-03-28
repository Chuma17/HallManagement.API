using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IBlockRepository
    {
        Task<Block> AddBlockAsync(Block request);
        Task<bool> Exists(Guid? blockId);
        Task<Block> UpdateBlockRoomCount(Guid blockId, Block request);
        Task<Block> GetBlockAsync(Guid? blockId);
        Task<Block> GetSingleBlockAsync(Guid blockId);
        Task<Block> DeleteBlockAsync(Guid blockId);
        Task<List<Block>> GetBlocksAsync();
        Task<List<Block>> GetBlocksInHall(Guid hallId);
    }
}
