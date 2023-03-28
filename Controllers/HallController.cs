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
        private readonly INotificationRepository _notificationRepository;
        private readonly IExitPassRepository _exitPassRepository;

        public HallController(IHallRepository hallRepository, IMapper mapper,
                                IHallAdminRepository hallAdminRepository, IHallTypeRepository hallTypeRepository,
                                IStudentDeviceRepository studentDeviceRepository, IStudentRepository studentRepository,
                                IRoomRepository roomRepository, IPorterRepository porterRepository,
                                IBlockRepository blockRepository, IComplaintFormRepository complaintFormRepository,
                                INotificationRepository notificationRepository, IExitPassRepository exitPassRepository)
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
            _notificationRepository = notificationRepository;
            _exitPassRepository = exitPassRepository;
        }

        //Retrieving all halls
        [HttpGet("get-all-halls")]
        public async Task<IActionResult> GetAllHalls()
        {
            var halls = await _hallRepository.GetHallsAsync();

            return Ok(halls);
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

            var hallAdmin = await _hallAdminRepository.GetHallAdminByHall(hallId);
            var hallType = await _hallTypeRepository.GetHallTypeForHall(hall.HallTypeId);            

            object hallDetails = new
            {
                hall.HallId,
                hallAdminName = hallAdmin.FirstName + " " + hallAdmin.LastName ?? "empty",
                HallType = hallType?.Description ?? "empty",
                hall.RoomCount,
                hall.AvailableRooms,
                hall.HallName,
                hall.HallGender,
                hall.RoomSpace,
                hall.StudentCount,
                hall.BlockCount,
                hall.IsAssigned
            };

            return Ok(hallDetails);
        }

        //Retrieving halls by gender
        [HttpGet("get-halls-by-gender"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<IActionResult> GetHallByGenderAsync()
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);
            var halls = await _hallRepository.GetHallsByGender(currentUserGender);

            if (halls == null)
            {
                return NotFound();
            }

            return Ok(halls);
        }

        //Retrieving assigned halls for porters
        [HttpGet("get-assigned-halls"), Authorize]
        public async Task<IActionResult> GetAssignedHallsForPorters()
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);
            var halls = await _hallRepository.GetAssignedHallsForPorters(currentUserGender);

            if (halls == null)
            {
                return NotFound();
            }

            return Ok(halls);
        }
        
        //Retrieving assigned halls
        [HttpGet("get-assigned-halls/{hallTypeId:guid}"), Authorize]
        public async Task<IActionResult> GetAssignedHalls([FromRoute] Guid hallTypeId)
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);
            var halls = await _hallRepository.GetAssignedHalls(currentUserGender, hallTypeId);

            if (halls == null)
            {
                return NotFound();
            }

            return Ok(halls);
        }

        //Retrieving unassigned halls
        [HttpGet("get-unassigned-halls"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<IActionResult> GetUnassignedHalls()
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);
            var halls = await _hallRepository.GetUnassignedHalls(currentUserGender);

            if (halls == null)
            {
                return NotFound();
            }

            return Ok(halls);
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
                    student.FirstName,
                    student.LastName,
                    student.StudyLevel,
                    student.Department,
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
                    porter.FirstName,
                    porter.LastName,
                };

                portersArray.Add(porterList);
            }

            return Ok(portersArray.ToArray());
        }

        //Retrieving rooms in a single hall
        [HttpGet("get-rooms-in-hall/{hallId:guid}")]
        public async Task<IActionResult> GetRoomsInHallAsync([FromRoute] Guid hallId)
        {
            var roomsInHall = await _roomRepository.GetRoomsInHall(hallId);

            if (roomsInHall == null)
            {
                return NotFound();
            }            

            return Ok(roomsInHall);
        }

        //Retrieving blocks in a single hall
        [HttpGet("get-blocks-in-hall/{hallId:guid}")]
        public async Task<IActionResult> GetBlocksInHallAsync([FromRoute] Guid hallId)
        {
            var blocksInHall = await _blockRepository.GetBlocksInHall(hallId);
            if (blocksInHall == null)
            {
                return NotFound();
            }

            return Ok(blocksInHall);
        }

        //Retrieving student devices in a single hall
        [HttpGet("get-studentDevices-in-hall/{hallId:guid}"), Authorize(Roles = "HallAdmin")]
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

            var halls = await _hallRepository.GetHallsAsync();
            foreach (var hall1 in halls)
            {
                if (request.HallName == hall1.HallName)
                {
                    return BadRequest("Hall already exists");
                }
            }

            var hallType = await _hallTypeRepository.GetHallTypeAsync(request.HallTypeId);
            var hallTypeRoomSpace = hallType.RoomSpaceCount;
            var hallGender = currentUserGender;

            var hall = await _hallRepository.AddHallAsync(_mapper.Map<Hall>(request));
            hall.RoomSpace = hallTypeRoomSpace;
            hall.HallGender = hallGender;
            hall.HallName = request.HallName.ToUpper();

            await _hallRepository.UpdateHall(hall.HallId, hall);
            return Ok("Hall added successfully");
        }

        //Delete hall
        [HttpDelete("delete-hall/{hallId:guid}")]
        public async Task<IActionResult> DeleteHallAsync([FromRoute] Guid hallId)
        {
            if (await _hallRepository.Exists(hallId))
            {
                var hall = await _hallRepository.GetHallAsync(hallId);
                if (hall.StudentCount != 0)
                {
                    return BadRequest("There are still students registered in this hall");
                }

                var hallAdmin = await _hallAdminRepository.GetHallAdminByHall(hallId);
                if (hallAdmin != null)
                {
                    await _hallAdminRepository.DeleteHallAdminAsync(hallAdmin.HallAdminId);
                }

                var complaints = await _complaintFormRepository.GetComplaintFormsInHall(hallId);
                foreach (var complaint in complaints)
                {
                    await _complaintFormRepository.DeleteComplaintForm(complaint.ComplaintFormId);
                }

                var notifications = await _notificationRepository.GetNotificationInHall(hallId);
                foreach (var notification in notifications)
                {
                    await _notificationRepository.DeleteNotification(notification.NotiFicationId);
                }

                var exitPasses = await _exitPassRepository.GetExitPassesInHall(hallId);
                foreach (var exitPass in exitPasses)
                {
                    await _exitPassRepository.DeleteExitPass(exitPass.ExitPassId);
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

                await _hallRepository.DeleteHallAsync(hallId);
                return Ok("You have deleted this hall");
            }

            return NotFound();
        }

    }
}
