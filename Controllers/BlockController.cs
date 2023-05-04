using AutoMapper;
using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Implementations;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : Controller
    {
        private readonly IBlockRepository _blockRepository;
        private readonly IMapper _mapper;
        private readonly IHallRepository _hallRepository;
        private readonly IRoomRepository _roomRepository;

        public BlockController(IBlockRepository blockRepository, IMapper mapper,
                                IHallRepository hallRepository, IRoomRepository roomRepository)
        {
            _blockRepository = blockRepository;
            _mapper = mapper;
            _hallRepository = hallRepository;
            _roomRepository = roomRepository;
        }

        //Retrieving a rooms in a block
        [HttpGet("get-rooms-in-block/{blockId:guid}")]
        public async Task<IActionResult> GetRoomsInBlockAsync([FromRoute] Guid blockId)
        {           
            var roomsInBlock = await _roomRepository.GetRoomsInBlockAsync(blockId);
            if (roomsInBlock == null)
            {
                return NotFound();
            }

            return Ok(roomsInBlock);
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
        [HttpPost("add-block"), Authorize(Roles = "HallAdmin")]
        public async Task<ActionResult<Block>> AddBlock([FromBody] AddBlockRequest request)
        {
            var hallId = User.FindFirstValue(ClaimTypes.UserData);
            if (!Guid.TryParse(hallId, out Guid currenthallId))
            {
                return Forbid();
            }

            var hallExists = await _hallRepository.Exists(request.HallId);
            if (!hallExists)
            {
                return BadRequest("The specified hall ID is invalid.");
            }
            var blocks = await _blockRepository.GetBlocksAsync();
            foreach (var block1 in blocks)
            {
                if (request.BlockName.ToUpper() == block1.BlockName.ToUpper() && Guid.Equals(block1.HallId, currenthallId))
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

            return Ok("Block Added Successfully");
        }        
    }
}
