using AutoMapper;
using HallManagementTest2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Repositories.Implementations;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IHallRepository _hallRepository;
        private readonly IHallTypeRepository _hallTypeRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IStudentRepository _studentRepository;

        public RoomController(IRoomRepository roomRepository, IMapper mapper,
                            IHallRepository hallRepository, IHallTypeRepository hallTypeRepository,
                            IBlockRepository blockRepository, IStudentRepository studentRepository)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _hallRepository = hallRepository;
            _hallTypeRepository = hallTypeRepository;
            _blockRepository = blockRepository;
            _studentRepository = studentRepository;
        }
        

        //Retrieving a single room
        [HttpGet("get-students-in-room/{roomId:guid}")]
        public async Task<IActionResult> GetRoomAsync([FromRoute] Guid roomId)
        {
            var room = await _roomRepository.GetRoomAsync(roomId);

            if (room == null)
            {
                return NotFound();
            }                        

            return Ok(room);
        }

        //Add room
        [HttpPost("add-room")]
        public async Task<ActionResult<Room>> AddRoom([FromBody] AddRoomRequest request)
        {
            var hallExists = await _hallRepository.Exists(request.HallId);
            if (!hallExists)
            {
                return BadRequest("The specified hall type ID is invalid.");
            }

            var hall = await _hallRepository.GetHallAsync(request.HallId);
            var block = await _blockRepository.GetBlockAsync(request.BlockId);

            var RoomSpace = block.RoomSpace;
            var RoomGender = block.BlockGender; 
            var blockName = block.BlockName;            

            var room = await _roomRepository.AddRoomAsync(_mapper.Map<Room>(request));

            hall.RoomCount += 1;
            hall.AvailableRooms += 1;

            block.RoomCount += 1;
            block.AvailableRooms += 1;

            var blockNumber = block.RoomCount.ToString();

            var roomNumber = blockName + blockNumber;

            room.MaxOccupants = RoomSpace;
            room.AvailableSpace = RoomSpace;
            room.RoomGender = RoomGender;
            room.RoomNumber = roomNumber;

            await _roomRepository.UpdateRoomCount(room.RoomId, room);
            await _hallRepository.UpdateRoomCount(request.HallId, hall);
            await _blockRepository.UpdateBlockRoomCount(request.BlockId, block);
            return Ok(room);
        }

        //Delete room
        [HttpDelete("delete-room/{roomId:guid}")]
        public async Task<IActionResult> DeleteRoomAsync([FromRoute] Guid roomId)
        {
            if (await _roomRepository.Exists(roomId))
            {
                var selectedRoom = await _roomRepository.GetRoomAsync(roomId);
                var block = await _blockRepository.GetBlockAsync(selectedRoom.BlockId);
                var hall = await _hallRepository.GetHallAsync(selectedRoom.HallId);

                var students = await _studentRepository.GetStudentsAsync();
                foreach (var student in students)
                {
                    if (student.RoomId == roomId)
                    {
                        student.RoomId = null;
                        selectedRoom.StudentCount -= 1;
                        block.StudentCount -= 1;
                        hall.StudentCount -= 1;

                        await _hallRepository.UpdateStudentCount(hall.HallId, hall);
                        await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);
                        await _studentRepository.JoinRoom(student.RoomId, student.StudentId);
                    }
                }

                block.RoomCount -= 1;
                hall.RoomCount -= 1;

                if (!selectedRoom.IsFull)
                {
                    block.AvailableRooms -= 1;
                    hall.AvailableRooms -= 1;
                }                

                var room = await _roomRepository.DeleteRoomAsync(roomId);
                await _hallRepository.UpdateRoomCount(hall.HallId, hall);
                await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);
                return Ok(_mapper.Map<Room>(room));
            }

            return NotFound();
        }

        //Updating room status
        [HttpPut("update-room-status/{roomId:guid}")]
        public async Task<IActionResult> UpdateRoomStatus([FromRoute] Guid roomId)
        {
            if (await _roomRepository.Exists(roomId))
            {
                var room = await _roomRepository.GetRoomAsync(roomId);
                if (room.IsUnderMaintenance)
                {
                    room.IsUnderMaintenance = false;
                    await _roomRepository.UpdateRoomStatus(roomId, room);
                }
                else
                {
                    room.IsUnderMaintenance = true;
                    await _roomRepository.UpdateRoomStatus(roomId, room);
                }
                return Ok(room.IsUnderMaintenance);
            }
            return NotFound();
        }
    }
}
