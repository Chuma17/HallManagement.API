using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using HallManagementTest2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HallManagementTest2.Requests;
using System.Security.Claims;
using HallManagementTest2.Repositories.Implementations;
using System.Security.Cryptography;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly AuthService _authService;
        private readonly IRoomRepository _roomRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly IStudentDeviceRepository _studentDeviceRepository;
        private readonly IExitPassRepository _exitPassRepository;
        private readonly IHallRepository _hallRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper,
                                 IRoleService roleService, IHallRepository hallRepository,
                                 AuthService authService, IRoomRepository roomRepository,
                                 IBlockRepository blockRepository, IStudentDeviceRepository studentDeviceRepository,
                                    IExitPassRepository exitPassRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _roleService = roleService;
            _authService = authService;
            _roomRepository = roomRepository;
            _blockRepository = blockRepository;
            _studentDeviceRepository = studentDeviceRepository;
            _exitPassRepository = exitPassRepository;
            _hallRepository = hallRepository;
        }

        //Getting the role of the user
        [HttpGet("get-roles"), Authorize(Roles = "Student")]
        public ActionResult<object> GetMe()
        {
            var role = _roleService.GetRole();
            return Ok(new { role });

        }


        //Retrieving all the students
        [HttpGet("get-all-students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetStudentsAsync();
            List<object> editedStudents = new List<object>();
            foreach (var student in students)
            {
                var editedStudent = new
                {
                    student.StudentId,
                    student.FirstName,
                    student.LastName,
                    student.Gender,
                    student.StudyLevel,
                    student.Course,
                    student.Department,
                    student.HallId,
                    student.RoomId,
                    student.Role
                };
                editedStudents.Add(editedStudent);
            }

            return Ok(editedStudents);
        }


        //Retrieving a single student
        [HttpGet("get-single-student/{studentId:guid}")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            var student = await _studentRepository.GetStudentAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }

            var block = await _blockRepository.GetBlockAsync(student.BlockId);
            var hall = await _hallRepository.GetHallAsync(student.HallId);
            var room = await _roomRepository.GetRoomAsync(student.RoomId);

            object studentDetails = new
            {
                student.StudentId,
                student.UserName,
                student.FirstName,
                student.LastName,
                student.Email,
                student.MatricNo,
                HallName = hall?.HallName ?? "empty",
                BlockName = block?.BlockName ?? "empty",
                RoomNumber = room?.RoomNumber ?? "empty",
                student.Gender,
                student.ProfileImageUrl,
                student.StudyLevel,
                student.Course,
                student.Department,
                student.Role
            };

            return Ok(studentDetails);
        }


        //Retrieving a single student devices
        [HttpGet("get-studentDevices"), Authorize(Roles = "Student")]
        public async Task<IActionResult> GetStudentDevicesAsync()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var student = await _studentRepository.GetStudentDevicesAsync(currentUserIdGuid);

            if (student == null)
            {
                return NotFound();
            }

            var studentDevices = student.StudentDevices;

            return Ok(studentDevices);
        }

        //Retrieving a single student exit passes
        [HttpGet("get-exitPasses"), Authorize(Roles = "Student")]
        public async Task<IActionResult> GetExitPassesAsync()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var student = await _studentRepository.GetExitPassesAsync(currentUserIdGuid);

            if (student == null)
            {
                return NotFound();
            }

            var studentExitPasses = student.ExitPasses;

            return Ok(studentExitPasses);
        }


        //Adding a student
        [HttpPost("student-registration")]
        public async Task<ActionResult<Student>> AddStudent([FromBody] AddStudentRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var student = _mapper.Map<Student>(request);
            student.PasswordHash = passwordHash;
            student.PasswordSalt = passwordSalt;

            await _studentRepository.AddStudentAsync(student);

            await _studentRepository.UpdateStudentPasswordHash(student.StudentId, student);

            return Ok("Student account created successfully");
        }


        //Deleting a student
        [HttpDelete("delete-student"), Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteStudentAsync()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            if (await _studentRepository.Exists(currentUserIdGuid))
            {
                var studentDevices = await _studentDeviceRepository.GetStudentDevicesForStudent(currentUserIdGuid);
                foreach (var device in studentDevices)
                {
                    await _studentDeviceRepository.DeleteStudentDeviceAsync(device.StudentDeviceId);
                }

                var student = await _studentRepository.GetStudentAsync(currentUserIdGuid);
                var hall = await _hallRepository.GetHallAsync(student.HallId);
                var exitPasses = await _exitPassRepository.GetExitPassesForStudent(student.MatricNo, hall.HallId);
                foreach (var exitPass in exitPasses)
                {
                    await _exitPassRepository.DeleteExitPass(exitPass.ExitPassId);
                }

                await _studentRepository.DeleteStudentAsync(currentUserIdGuid);
                return Ok("This Student account has been deleted");
            }

            return NotFound();
        }


        //Updating a student Record
        [HttpPut("update-student"), Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateStudentAsync([FromBody] UpdateStudentRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            if (await _studentRepository.Exists(currentUserIdGuid))
            {
                //Update Details
                var updatedStudent = await _studentRepository.UpdateStudent(currentUserIdGuid, _mapper.Map<Student>(request));

                if (updatedStudent != null)
                {
                    return Ok("Account Updated successfully");
                }
            }

            return NotFound();
        }
        
        //Updating a student Status
        [HttpPut("update-student-status/{studentId:guid}"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> UpdateStudentStatus([FromRoute] Guid studentId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            if (await _studentRepository.Exists(studentId))
            {                
                var student = await _studentRepository.GetStudentAsync(studentId);
                if (student.IsBlocked)
                {
                    student.IsBlocked = false;
                    return Ok("Student has beed unblocked");
                }
                else
                {
                    student.IsBlocked = true;
                    if (student.RoomId != Guid.Empty)
                    {
                        await LeaveRoom();
                    }

                    else if (student.BlockId != Guid.Empty)
                    {
                        await LeaveBlock();
                    }

                    else if (student.HallId != Guid.Empty)
                    {
                        await LeaveHall();
                    }

                    return Ok("Student has been blocked successfully");
                }
            }

            return NotFound();
        }


        //Join Hall
        [HttpPost("join-hall"), Authorize(Roles = "Student")]
        public async Task<IActionResult> JoinHall([FromBody] JoinRequest joinHallRequest)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var student = await _studentRepository.Exists(currentUserIdGuid);
            if (!student)
            {
                return NotFound();
            }

            var hallExists = await _hallRepository.Exists(joinHallRequest.HallId);
            if (!hallExists)
            {
                return BadRequest("The specified hall is invalid.");
            }

            var existingStudent = await _studentRepository.GetStudentAsync(currentUserIdGuid);

            if (existingStudent.IsBlocked == true)
            {
                return BadRequest("You cannot select this hall because you have been blocked! Meet the hall admin.");
            }

            if (existingStudent.HallId == joinHallRequest.HallId)
            {
                return BadRequest("You are already registered in this hall");
            }

            if (existingStudent.HallId != null)
            {
                return BadRequest("You are registered in another hall");
            }

            var UserGender = User.FindFirstValue(ClaimTypes.Gender);
            var hall = await _hallRepository.GetHallAsync(joinHallRequest.HallId);
            if (!hall.IsAssigned)
            {
                return BadRequest("This Hall is not available at the moment");
            }

            if (hall.BlockCount == 0)
            {
                return BadRequest("There are no blocks in this hall");
            }

            if (hall.AvailableRooms == 0)
            {
                return BadRequest("There are no available rooms at the moment");
            }

            if (UserGender != hall.HallGender)
            {
                return BadRequest("You cannot join a hall for the opposite gender");
            }

            hall.StudentCount += 1;

            await _studentRepository.JoinHall(joinHallRequest.HallId, currentUserIdGuid);
            await _hallRepository.UpdateStudentCount(joinHallRequest.HallId, hall);
            return Ok("You have successfully Joined this hall");
        }


        //Leave Hall
        [HttpPut("leave-hall"), Authorize(Roles = "Student")]
        public async Task<IActionResult> LeaveHall()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var student = await _studentRepository.Exists(currentUserIdGuid);
            if (!student)
            {
                return NotFound();
            }

            var existingStudent = await _studentRepository.GetStudentAsync(currentUserIdGuid);
            var roomId = existingStudent.RoomId;
            var blockId = existingStudent.BlockId;
            var hallId = existingStudent.HallId;
            var hall = await _hallRepository.GetHallAsync(hallId);

            if (blockId != null)
            {
                return BadRequest("You have to leave the block before you can leave the hall");
            }

            if (roomId != null)
            {
                return BadRequest("You have to leave the room and block before you can leave the hall");
            }

            if (hallId != null)
            {
                hallId = null;

                hall.StudentCount -= 1;

                var studentDevices = await _studentDeviceRepository.GetStudentDevicesByMatricNo(existingStudent.MatricNo);
                foreach (var device in studentDevices)
                {
                    await _studentDeviceRepository.DeleteStudentDeviceAsync(device.StudentDeviceId);
                }

                await _studentRepository.JoinHall(hallId, currentUserIdGuid);
                await _hallRepository.UpdateStudentCount(hallId, hall);
                return Ok("You have left the hall");
            }

            return BadRequest("You are not registered in any hall");
        }


        //Join Block
        [HttpPost("join-block"), Authorize(Roles = "Student")]
        public async Task<IActionResult> JoinBlock([FromBody] JoinRequest joinRequest)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var student = await _studentRepository.Exists(currentUserIdGuid);
            if (!student)
            {
                return NotFound();
            }

            var blockExists = await _blockRepository.Exists(joinRequest.BlockId);
            if (!blockExists)
            {
                return BadRequest("The specified block is invalid.");
            }

            var existingStudent = await _studentRepository.GetStudentAsync(currentUserIdGuid);
            var hallId = existingStudent.HallId;

            if (hallId == null)
            {
                return BadRequest("You have to join a hall before you join a block");
            }

            if (existingStudent.BlockId == joinRequest.BlockId)
            {
                return BadRequest("You are already registered in this block");
            }

            if (existingStudent.BlockId != null)
            {
                return BadRequest("You are already registered in another block");
            }

            var block = await _blockRepository.GetBlockAsync(joinRequest.BlockId);

            if (block.RoomCount == 0)
            {
                return BadRequest("There are no rooms in this block");
            }

            if (block.AvailableRooms == 0)
            {
                return BadRequest("Block is full");
            }

            block.StudentCount += 1;

            await _studentRepository.JoinBlock(joinRequest.BlockId, currentUserIdGuid);
            await _blockRepository.UpdateBlockRoomCount(joinRequest.BlockId, block);
            return Ok("You have successfully Joined the block");
        }


        //Leave Block
        [HttpPut("leave-block"), Authorize(Roles = "Student")]
        public async Task<IActionResult> LeaveBlock()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var student = await _studentRepository.Exists(currentUserIdGuid);
            if (!student)
            {
                return NotFound();
            }

            var existingStudent = await _studentRepository.GetStudentAsync(currentUserIdGuid);
            var roomId = existingStudent.RoomId;
            var blockId = existingStudent.BlockId;

            if (roomId != null)
            {
                return BadRequest("You have to leave the room before you can leave the block");
            }

            if (blockId != null)
            {
                var block = await _blockRepository.GetBlockAsync(blockId);
                block.StudentCount -= 1;
                blockId = null;

                await _studentRepository.JoinBlock(blockId, currentUserIdGuid);
                await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);

                return Ok("You have left the block");
            }

            return BadRequest("You are not registered in any block");
        }


        //Join Room
        [HttpPost("join-room"), Authorize(Roles = "Student")]
        public async Task<IActionResult> JoinRoom([FromBody] JoinRequest joinRequest)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var student = await _studentRepository.Exists(currentUserIdGuid);
            if (!student)
            {
                return NotFound();
            }

            var roomExists = await _roomRepository.Exists(joinRequest.RoomId);
            if (!roomExists)
            {
                return BadRequest("The specified room is invalid.");
            }

            var room = await _roomRepository.GetRoomAsync(joinRequest.RoomId);
            if (room.IsUnderMaintenance)
            {
                return BadRequest("Room is under maintenance. Please select another room");
            }

            var existingStudent = await _studentRepository.GetStudentAsync(currentUserIdGuid);
            var blockId = existingStudent.BlockId;
            var hallId = existingStudent.HallId;

            if (hallId == null)
            {
                return BadRequest("You have to join a hall and a block before you join a room");
            }

            if (blockId == null)
            {
                return BadRequest("You have to join a block before you join a room");
            }

            if (existingStudent.RoomId == joinRequest.RoomId)
            {
                return BadRequest("You are already registered in this room");
            }

            if (existingStudent.RoomId != null)
            {
                return BadRequest("You are already registered in another room");
            }

            var block = await _blockRepository.GetBlockAsync(room.BlockId);

            if (room.AvailableSpace == 0)
            {
                return BadRequest("Room is full");
            }

            room.AvailableSpace -= 1;
            room.StudentCount += 1;

            await _studentRepository.JoinRoom(joinRequest.RoomId, currentUserIdGuid);
            await _roomRepository.UpdateAvailableSpace(joinRequest.RoomId, room);

            if (room.AvailableSpace == 0 && room.StudentCount == room.MaxOccupants)
            {
                room.IsFull = true;

                if (block.AvailableRooms != 0)
                {
                    block.AvailableRooms -= 1;
                    await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);
                }

                await _roomRepository.UpdateAvailableSpace(joinRequest.RoomId, room);
            }

            return Ok("You have successfully Joined the room");
        }


        //Leave Room
        [HttpPut("leave-room"), Authorize(Roles = "Student")]
        public async Task<IActionResult> LeaveRoom()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var student = await _studentRepository.Exists(currentUserIdGuid);
            if (!student)
            {
                return NotFound();
            }

            var existingStudent = await _studentRepository.GetStudentAsync(currentUserIdGuid);

            var roomId = existingStudent.RoomId;
            var roomExists = await _roomRepository.Exists(roomId);
            if (!roomExists)
            {
                return BadRequest("You are not registered in any room");
            }

            var blockId = existingStudent.BlockId;
            var hallId = existingStudent.HallId;

            if (roomId != null)
            {

                var room = await _roomRepository.GetRoomAsync(roomId);
                var block = await _blockRepository.GetBlockAsync(blockId);
                var hall = await _hallRepository.GetHallAsync(hallId);
                roomId = null;

                if (room.IsFull)
                {
                    room.AvailableSpace += 1;
                    room.StudentCount -= 1;
                    room.IsFull = false;
                    hall.AvailableRooms += 1;
                    block.AvailableRooms += 1;

                    await _studentRepository.JoinRoom(roomId, currentUserIdGuid);
                    await _roomRepository.UpdateAvailableSpace(roomId, room);
                    await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);
                    await _hallRepository.UpdateRoomCount(hall.HallId, hall);

                    return Ok("You have left the room");
                }

                else
                {
                    room.AvailableSpace += 1;
                    room.StudentCount -= 1;

                    await _studentRepository.JoinRoom(roomId, currentUserIdGuid);
                    await _roomRepository.UpdateAvailableSpace(roomId, room);

                    return Ok("You have left the room");
                }
            }

            return BadRequest("You are not registered in any room");
        }

        //Student login 
        [HttpPost("student-login")]
        public async Task<ActionResult<Student>> Login([FromBody] StudentLoginRequest loginRequest)
        {
            var student = await _studentRepository.GetStudentByMatricNo(loginRequest.MatricNo);
            if (student == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            if (!_authService.VerifyPasswordHash(loginRequest.Password, student.PasswordHash, student.PasswordSalt))
                return BadRequest(new { message = "UserName or password is incorrect" });

            string token = _authService.CreateStudentToken(student);
            student.AccessToken = token;

            var refreshToken = _authService.GenerateRefreshToken();
            _authService.SetStudentRefreshToken(refreshToken, student, HttpContext);

            await _studentRepository.UpdateStudentToken(student.MatricNo, student);

            var block = await _blockRepository.GetBlockAsync(student.BlockId);
            var hall = await _hallRepository.GetHallAsync(student.HallId);
            var room = await _roomRepository.GetRoomAsync(student.RoomId);

            object studentDetails = new
            {
                student.StudentId,
                student.UserName,
                student.Gender,
                student.FirstName,
                student.LastName,
                student.HallId,
                student.BlockId,
                student.RoomId,
                HallName = hall?.HallName ?? "empty",
                BlockName = block?.BlockName ?? "empty",
                RoomNumber = room?.RoomNumber ?? "empty",
                student.StudyLevel,
                student.Course,
                student.Department,
                student.Role,
                student.Email,
                student.AccessToken,
                student.RefreshToken,
                student.ProfileImageUrl
            };

            return Ok(studentDetails);
        }

        [HttpPost("student-refresh-token/{studentId:guid}")]
        public async Task<ActionResult<string>> RefreshToken([FromRoute] Guid studentId)
        {
            var student = await _studentRepository.GetStudentAsync(studentId);

            if (student == null)
            {
                return NotFound();
            }

            var refreshToken = Request.Cookies["refreshToken"];

            if (!student.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (student.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            string token = _authService.CreateStudentToken(student);
            student.AccessToken = token;

            var newRefreshToken = _authService.GenerateRefreshToken();
            _authService.SetStudentRefreshToken(newRefreshToken, student, HttpContext);

            await _studentRepository.UpdateStudentToken(student.MatricNo, student);

            return Ok(new { token });
        }

        [HttpPost("student-logout")]
        public IActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { message = "User is not authenticated" });
            }

            Response.Cookies.Delete("refreshToken"); // Remove the refresh token cookie

            return Ok(new { message = "Logout successful" });
        }
    }
}