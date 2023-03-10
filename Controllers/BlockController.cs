using AutoMapper;
using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Implementations;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : Controller
    {
        private readonly IBlockRepository _blockRepository;
        private readonly IMapper _mapper;
        private readonly IHallRepository _hallRepository;

        public BlockController(IBlockRepository blockRepository, IMapper mapper,
                                IHallRepository hallRepository)
        {
            _blockRepository = blockRepository;
            _mapper = mapper;
            _hallRepository = hallRepository;
        }

        //Retrieving a rooms in a block
        [HttpGet("get-rooms-in-block/{blockId:guid}")]
        public async Task<IActionResult> GetRoomsInBlockAsync([FromRoute] Guid blockId)
        {
            var block = await _blockRepository.GetBlockAsync(blockId);

            if (block == null)
            {
                return NotFound();
            }

            return Ok(block.Rooms);
        }
        
        //Retrieving a single block
        [HttpGet("get-single-block/{blockId:guid}")]
        public async Task<IActionResult> GetBlockAsync([FromRoute] Guid blockId)
        {
            var block = await _blockRepository.GetSingleBlockAsync(blockId);

            if (block == null)
            {
                return NotFound();
            }

            return Ok(block);
        }

        //Add block
        [HttpPost("add-block")]
        public async Task<ActionResult<Block>> AddBlock([FromBody] AddBlockRequest request)
        {
            var hallExists = await _hallRepository.Exists(request.HallId);
            if (!hallExists)
            {
                return BadRequest("The specified hall ID is invalid.");
            }
            var blocks = await _blockRepository.GetBlocksAsync();
            foreach (var block1 in blocks)
            {
                if (request.BlockName.ToUpper() == block1.BlockName.ToUpper())
                {
                    return BadRequest("Block name already exists");
                }
            }            

            var hall = await _hallRepository.GetHallAsync(request.HallId);

            var block = await _blockRepository.AddBlockAsync(_mapper.Map<Block>(request));

            block.BlockName = request.BlockName.ToUpper();
            hall.BlockCount += 1;
            block.BlockGender = hall.HallGender;
            block.RoomSpace = hall.RoomSpace;

            await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);
            await _hallRepository.UpdateBlockCount(request.HallId, hall);

            return Ok(block);
        }        
    }
}
