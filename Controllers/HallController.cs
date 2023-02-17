using AutoMapper;
using HallManagementTest2.Models;
using Microsoft.AspNetCore.Mvc;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallController : Controller
    {
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;
        private readonly IHallAdminRepository _hallAdminRepository;
        private readonly IHallTypeRepository _hallTypeRepository;
        private readonly IStudentDeviceRepository _studentDeviceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IPorterRepository _porterRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IComplaintFormRepository _complaintFormRepository;

        public HallController(IHallRepository hallRepository, IMapper mapper,
                                IHallAdminRepository hallAdminRepository, IHallTypeRepository hallTypeRepository,
                                IStudentDeviceRepository studentDeviceRepository, IStudentRepository studentRepository,
                                IRoomRepository roomRepository, IPorterRepository porterRepository,
                                IBlockRepository blockRepository, IComplaintFormRepository complaintFormRepository)
        {
            _hallRepository = hallRepository;
            _mapper = mapper;
            _hallAdminRepository = hallAdminRepository;
            _hallTypeRepository = hallTypeRepository;
            _studentDeviceRepository = studentDeviceRepository;
            _studentRepository = studentRepository;
            _roomRepository = roomRepository;
            _porterRepository = porterRepository;
            _blockRepository = blockRepository;
            _complaintFormRepository = complaintFormRepository;
        }

        //Retrieving all halls
        [HttpGet("get-all-halls")]
        public async Task<IActionResult> GetAllHalls()
        {
            var hall = await _hallRepository.GetHallsAsync();

            return Ok(hall);
        }

        //Retrieving a single hall
        [HttpGet("get-single-hall/{hallId:guid}")]
        public async Task<IActionResult> GetHallAsync([FromRoute] Guid hallId)
        {
            var hall = await _hallRepository.GetHallAsync(hallId);

            if (hall == null)
            {
                return NotFound();
            }

            return Ok(hall);
        }

        //Retrieving a single hall by gender
        [HttpGet("get-halls-by-gender"), Authorize(Roles = "Student")]
        public async Task<IActionResult> GetHallByGenderAsync()
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);
            var hall = await _hallRepository.GetHallsByGender(currentUserGender);

            if (hall == null)
            {
                return NotFound();
            }

            return Ok(hall);
        }

        //Retrieving students in a single hall
        [HttpGet("get-students-in-hall/{hallId:guid}")]
        public async Task<IActionResult> GetStudentsInHallAsync([FromRoute] Guid hallId)
        {
            var studentsInHall = await _hallRepository.GetStudentsInHallAsync(hallId);

            if (studentsInHall == null)
            {
                return NotFound();
            }

            var students = studentsInHall.Students;

            var studentsArray = new List<object>();

            foreach (var student in students)
            {
                var studentList = new
                {
                    student.StudentId,
                    student.UserName,
                    student.Gender,
                    student.FirstName,
                    student.LastName,
                    student.DateOfBirth,
                    student.Mobile,
                    student.StudyLevel,
                    student.Address,
                    student.Course,
                    student.Department,
                    student.School,
                    student.State,
                };

                studentsArray.Add(studentList);
            }

            return Ok(studentsArray.ToArray());
        }

        //Retrieving porters in a single hall
        [HttpGet("get-porters-in-hall/{hallId:guid}")]
        public async Task<IActionResult> GetPortersInHallAsync([FromRoute] Guid hallId)
        {
            var portersInHall = await _hallRepository.GetPortersInHallAsync(hallId);

            if (portersInHall == null)
            {
                return NotFound();
            }

            var porters = portersInHall.Porters;

            var portersArray = new List<object>();

            foreach (var porter in porters)
            {
                var porterList = new
                {
                    porter.PorterId,
                    porter.UserName,
                    porter.Gender,
                    porter.FirstName,
                    porter.LastName,
                    porter.DateOfBirth,
                    porter.Mobile,
                    porter.Address,
                    porter.State
                };

                portersArray.Add(porterList);
            }

            return Ok(portersArray.ToArray());
        }

        //Retrieving rooms in a single hall
        [HttpGet("get-rooms-in-hall/{hallId:guid}")]
        public async Task<IActionResult> GetRoomsInHallAsync([FromRoute] Guid hallId)
        {
            var roomsInHall = await _hallRepository.GetRoomsInHallAsync(hallId);

            if (roomsInHall == null)
            {
                return NotFound();
            }

            var rooms = roomsInHall.Rooms;

            return Ok(rooms);
        }

        //Retrieving blocks in a single hall
        [HttpGet("get-blocks-in-hall/{hallId:guid}")]
        public async Task<IActionResult> GetBlocksInHallAsync([FromRoute] Guid hallId)
        {
            var blocksInHall = await _hallRepository.GetBlocksInHallAsync(hallId);

            if (blocksInHall == null)
            {
                return NotFound();
            }

            var blocks = blocksInHall.Blocks;

            return Ok(blocks);
        }

        //Retrieving student devices in a single hall
        [HttpGet("get-studentDevices-in-hall/{hallId:guid}")]
        public async Task<IActionResult> GetStudentDevicesInHallAsync([FromRoute] Guid hallId)
        {
            var studentDevicesInHall = await _hallRepository.GetStudentDevicesInHallAsync(hallId);

            if (studentDevicesInHall == null)
            {
                return NotFound();
            }

            var devices = studentDevicesInHall.StudentDevices;

            return Ok(devices);
        }

        //Retrieving complaint forms in a single hall
        [HttpGet("get-complaintForms-in-hall/{hallId:guid}")]
        public async Task<IActionResult> GetComplaintFormsInHallAsync([FromRoute] Guid hallId)
        {
            var complaintFormsInHall = await _hallRepository.GetComplaintFormsInHallAsync(hallId);

            if (complaintFormsInHall == null)
            {
                return NotFound();
            }

            var complaints = complaintFormsInHall.ComplaintForms;

            return Ok(complaints);
        }

        //Add hall
        [HttpPost("add-hall"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<ActionResult<Hall>> AddHall([FromBody] AddHallRequest request)
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);

            var hallTypeExists = await _hallTypeRepository.Exists(request.HallTypeId);
            if (!hallTypeExists)
            {
                return BadRequest("The specified hall type ID is invalid.");
            }

            var hallType = await _hallTypeRepository.GetHallTypeAsync(request.HallTypeId);
            var hallTypeRoomSpace = hallType.RoomSpaceCount;
            var hallGender = currentUserGender;

            var hall = await _hallRepository.AddHallAsync(_mapper.Map<Hall>(request));
            hall.RoomSpace = hallTypeRoomSpace;
            hall.HallGender = hallGender;

            await _hallRepository.UpdateHall(hall.HallId, hall);
            return Ok(hall);
        }

        //Delete hall
        [HttpDelete("delete-hall/{hallId:guid}")]
        public async Task<IActionResult> DeleteHallAsync([FromRoute] Guid hallId)
        {
            if (await _hallRepository.Exists(hallId))
            {
                var studentDevices = await _studentDeviceRepository.GetStudentDevicesInHall(hallId);
                foreach (var device in studentDevices)
                {
                    await _studentDeviceRepository.DeleteStudentDeviceAsync(device.StudentDeviceId);
                }

                var students = await _studentRepository.GetStudentsInHall(hallId);
                foreach (var student in students)
                {
                    await _studentRepository.DeleteStudentAsync(student.StudentId);
                }

                var complaints = await _complaintFormRepository.GetComplaintFormsInHall(hallId);
                foreach (var complaint in complaints)
                {
                    await _complaintFormRepository.DeleteComplaintForm(complaint.ComplaintFormId);
                }

                var porters = await _porterRepository.GetPortersInHall(hallId);
                foreach (var porter in porters)
                {
                    await _porterRepository.DeletePorterAsync(porter.PorterId);
                }

                var rooms = await _roomRepository.GetRoomsInHall(hallId);
                foreach (var room in rooms)
                {
                    await _roomRepository.DeleteRoomAsync(room.RoomId);
                }

                var blocks = await _blockRepository.GetBlocksInHall(hallId);
                foreach (var block in blocks)
                {
                    await _blockRepository.DeleteBlockAsync(block.BlockId);
                }

                var hall = await _hallRepository.DeleteHallAsync(hallId);
                return Ok("You have deleted this hall");
            }

            return NotFound();
        }

        //Updating a Hall Record
        [HttpPut("update-hall/{hallId:guid}"), Authorize(Roles = "ChiefHallAdmin,HallAdmin")]
        public async Task<IActionResult> UpdateRoomAsync([FromRoute] Guid hallId, [FromBody] UpdateHallRequest request)
        {
            if (await _hallRepository.Exists(hallId))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
                {
                    var hallAdmin = await _hallAdminRepository.GetHallAdminByHall(hallId);

                    if (hallAdmin != null)
                    {
                        if (hallAdmin.HallAdminId != currentUserIdGuid)
                        {
                            return BadRequest("You are not the Hall Admin assigned to this hall");
                        }
                    }
                    else
                    {
                        return BadRequest("You are not the Hall Admin assigned to this hall");
                    }

                }
                else
                {
                    return Forbid();
                }


                //Update Details
                var updatedHall = await _hallRepository.UpdateHall(hallId, _mapper.Map<Hall>(request));

                if (updatedHall != null)
                {
                    return Ok(_mapper.Map<Hall>(updatedHall));
                }
            }

            return NotFound();
        }
    }
}
