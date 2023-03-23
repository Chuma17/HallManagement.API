using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintFormController : Controller
    {
        private readonly IComplaintFormRepository _complaintFormRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IBlockRepository _blockRepository;

        public ComplaintFormController(IComplaintFormRepository complaintFormRepository, IHallRepository hallRepository,
                                        IMapper mapper, IStudentRepository studentRepository,
                                        IRoomRepository roomRepository, IBlockRepository blockRepository)
        {
            _complaintFormRepository = complaintFormRepository;
            _hallRepository = hallRepository;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _roomRepository = roomRepository;
            _blockRepository = blockRepository;
        }

        //Add complaint form
        [HttpPost("add-complaintForm"), Authorize(Roles = "Student")]
        public async Task<ActionResult<ComplaintForm>> AddComplaintForm([FromBody] AddComplaintFormRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                if (request == null)
                {
                    return BadRequest();
                }

                var student = await _studentRepository.GetStudentAsync(currentUserIdGuid);
                if (student == null)
                {
                    return NotFound();
                }

                if (student.HallId == null)
                {
                    return BadRequest("You must be registered in a hall");
                }

                if (student.BlockId == null)
                {
                    return BadRequest("You must be registered in a block");
                }

                if (student.RoomId == null)
                {
                    return BadRequest("You must be registered in a room");
                }

                var hall = await _hallRepository.GetHallAsync(student.HallId);
                var block = await _blockRepository.GetBlockAsync(student.BlockId);
                var room = await _roomRepository.GetRoomAsync(student.RoomId);

                var complaint = _mapper.Map<ComplaintForm>(request);
                complaint.HallId = hall.HallId;
                complaint.BlockId = block.HallId;
                complaint.RoomId = room.RoomId;
                complaint.RoomNumber = room.RoomNumber;

                var complaintForm = await _complaintFormRepository.AddComplaintFormAsync(complaint, hall.HallId);
                await _complaintFormRepository.UpdateComplaintForm(complaintForm.ComplaintFormId, complaintForm);

                return Ok("Complaint has been submitted successfully!");
            }
            return BadRequest("User must be authenticated first");
        }

        //Get Complaints in block
        [HttpGet("get-complaints-in-block/{blockId:guid}"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> GetComplaintsInBlock([FromRoute] Guid blockId)
        {
            var block = await _blockRepository.GetBlockAsync(blockId);
            if (block == null)
            {
                return BadRequest("Block does not exist");
            }

            var complaints = await _complaintFormRepository.GetComplaintFormsInBlock(blockId);            

            return Ok(complaints);
        }

        //Get Complaints in room
        [HttpGet("get-complaints-in-room/{roomId:guid}"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> GetComplaintsInRoom([FromRoute] Guid roomId)
        {
            var room = await _roomRepository.GetRoomAsync(roomId);
            if (room == null)
            {
                return BadRequest("Room does not exist");
            }

            var complaints = await _complaintFormRepository.GetComplaintFormsInRoom(roomId);

            return Ok(complaints);
        }
    }
}
