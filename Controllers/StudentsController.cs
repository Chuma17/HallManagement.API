using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using HallManagementTest2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HallManagementTest2.Requests;

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
        private readonly IHallRepository _hallRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper,
                                 IRoleService roleService, IHallRepository hallRepository,
                                 AuthService authService, IRoomRepository roomRepository,
                                 IBlockRepository blockRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _roleService = roleService;
            _authService = authService;
            _roomRepository = roomRepository;
            _blockRepository = blockRepository;
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

            return Ok(students);
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

            object studentDetails = new
            {
                student.StudentId,
                student.UserName,
                student.FirstName,
                student.LastName,
                student.DateOfBirth,
                student.Gender,
                student.ProfileImageUrl,
                student.Mobile,
                student.StudyLevel,
                student.Address,
                student.Course,
                student.Department,
                student.School,
                student.State,
                student.HallId,
                student.RoomId,
                student.Role,
            };

            return Ok(studentDetails);
        }


        //Retrieving a single student devices
        [HttpPost("get-studentDevices")]
        public async Task<IActionResult> GetStudentAsync([FromBody] string userName)
        {
            var student = await _studentRepository.GetStudentByUserName(userName);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
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

            var student = await _studentRepository.AddStudentAsync(_mapper.Map<Student>(request));

            student.PasswordHash = passwordHash;
            student.PasswordSalt = passwordSalt;

            await _studentRepository.UpdateStudentPasswordHash(student.StudentId, student);

            object studentDetails = new
            {
                student.StudentId, student.UserName,
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
                student.HallId,
                student.RoomId,
                student.Role,
                student.AccessToken,
                student.ProfileImageUrl
            };

            return Ok(new { studentDetails });
        }


        //Deleting a student
        [HttpDelete("delete-student/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await _studentRepository.Exists(studentId))
            {
                var student = await _studentRepository.DeleteStudentAsync(studentId);
                return Ok(_mapper.Map<Student>(student));
            }

            return NotFound();
        }


        //Updating a student Record
        [HttpPut("update-student/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await _studentRepository.Exists(studentId))
            {
                //Update Details
                var updatedStudent = await _studentRepository.UpdateStudent(studentId, _mapper.Map<Student>(request));

                if (updatedStudent != null)
                {
                    var UpdatedStudent = _mapper.Map<Student>(updatedStudent);

                    object updatedStudentDetails = new
                    {
                        UpdatedStudent.UserName,
                        UpdatedStudent.Gender,
                        UpdatedStudent.FirstName,
                        UpdatedStudent.LastName,
                        UpdatedStudent.DateOfBirth,
                        UpdatedStudent.Mobile,
                        UpdatedStudent.StudyLevel,
                        UpdatedStudent.Address,
                        UpdatedStudent.Course,
                        UpdatedStudent.Department,
                        UpdatedStudent.School,
                        UpdatedStudent.State,
                        UpdatedStudent.Role
                    };

                    return Ok(updatedStudentDetails);
                }
            }

            return NotFound();
        }


        //Join Hall
        [HttpPost("join-hall/{studentId:guid}")]
        public async Task<IActionResult> JoinHall([FromBody] Guid hallId, [FromRoute] Guid studentId)
        {
            var student = await _studentRepository.Exists(studentId);
            if (!student)
            {
                return NotFound();
            }

            var hallExists = await _hallRepository.Exists(hallId);
            if (!hallExists)
            {
                return BadRequest("The specified hall is invalid.");
            }

            var existingStudent = await _studentRepository.GetStudentAsync(studentId);

            if (existingStudent.HallId == hallId)
            {
                return BadRequest("You are already registered in this hall");
            }

            if (existingStudent.HallId != null)
            {
                return BadRequest("You are already registered in another hall");
            }

            var hall = await _hallRepository.GetHallAsync(hallId);
            hall.StudentCount += 1;

            await _studentRepository.JoinHall(hallId, studentId);
            await _hallRepository.UpdateStudentCount(hallId, hall);
            return Ok("You have successfully Joined the hall");
        }


        //Leave Hall
        [HttpPut("leave-hall/{studentId:guid}")]
        public async Task<IActionResult> LeaveHall([FromRoute] Guid studentId)
        {
            var student = await _studentRepository.Exists(studentId);
            if (!student)
            {
                return NotFound();
            }

            var existingStudent = await _studentRepository.GetStudentAsync(studentId);
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

                await _studentRepository.JoinHall(hallId, studentId);
                await _hallRepository.UpdateStudentCount(hallId, hall);
                return Ok("You have left the hall");
            }

            return BadRequest("You are not registered in any hall");
        }


        //Join Block
        [HttpPost("join-block/{studentId:guid}")]
        public async Task<IActionResult> JoinBlock([FromBody] Guid blockId, [FromRoute] Guid studentId)
        {
            var student = await _studentRepository.Exists(studentId);
            if (!student)
            {
                return NotFound();
            }

            var blockExists = await _blockRepository.Exists(blockId);
            if (!blockExists)
            {
                return BadRequest("The specified block is invalid.");
            }

            var existingStudent = await _studentRepository.GetStudentAsync(studentId);
            var hallId = existingStudent.HallId;

            if (hallId == null)
            {
                return BadRequest("You have to join a hall before you join a block");
            }

            if (existingStudent.BlockId == blockId)
            {
                return BadRequest("You are already registered in this block");
            }

            if (existingStudent.BlockId != null)
            {
                return BadRequest("You are already registered in another block");
            }

            var block = await _blockRepository.GetBlockAsync(blockId);

            if (block.AvailableRooms == 0)
            {
                return BadRequest("Block is full");
            }

            block.StudentCount += 1;

            await _studentRepository.JoinBlock(blockId, studentId);
            await _blockRepository.UpdateBlockRoomCount(blockId, block);
            return Ok("You have successfully Joined the block");
        }


        //Leave Block
        [HttpPut("leave-block/{studentId:guid}")]
        public async Task<IActionResult> LeaveBlock([FromRoute] Guid studentId)
        {
            var student = await _studentRepository.Exists(studentId);
            if (!student)
            {
                return NotFound();
            }

            var existingStudent = await _studentRepository.GetStudentAsync(studentId);
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

                await _studentRepository.JoinBlock(blockId, studentId);
                await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);

                return Ok("You have left the block");
            }

            return BadRequest("You are not registered in any block");
        }


        //Join Room
        [HttpPost("join-room/{studentId:guid}")]
        public async Task<IActionResult> JoinRoom([FromBody] Guid roomId, [FromRoute] Guid studentId)
        {
            var student = await _studentRepository.Exists(studentId);
            if (!student)
            {
                return NotFound();
            }

            var roomExists = await _roomRepository.Exists(roomId);
            if (!roomExists)
            {
                return BadRequest("The specified room is invalid.");
            }

            var room = await _roomRepository.GetRoomAsync(roomId);
            if (room.IsUnderMaintenance)
            {
                return BadRequest("Room is under maintenance. Please select another room");
            }

            var existingStudent = await _studentRepository.GetStudentAsync(studentId);
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

            if (existingStudent.RoomId == roomId)
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

            await _studentRepository.JoinRoom(roomId, studentId);
            await _roomRepository.UpdateAvailableSpace(roomId, room);

            if (room.AvailableSpace == 0 && room.StudentCount == room.MaxOccupants)
            {
                room.IsFull = true;

                if (block.AvailableRooms != 0)
                {
                    block.AvailableRooms -= 1;
                    await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);
                }

                await _roomRepository.UpdateAvailableSpace(roomId, room);
            }

            return Ok("You have successfully Joined the room");
        }


        //Leave Room
        [HttpPut("leave-room/{studentId:guid}")]
        public async Task<IActionResult> LeaveRoom([FromRoute] Guid studentId)
        {
            var student = await _studentRepository.Exists(studentId);
            if (!student)
            {
                return NotFound();
            }

            var existingStudent = await _studentRepository.GetStudentAsync(studentId);

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

                    await _studentRepository.JoinRoom(roomId, studentId);
                    await _roomRepository.UpdateAvailableSpace(roomId, room);
                    await _blockRepository.UpdateBlockRoomCount(block.BlockId, block);
                    await _hallRepository.UpdateRoomCount(hall.HallId, hall);

                    return Ok("You have left the room");
                }

                else
                {
                    room.AvailableSpace += 1;
                    room.StudentCount -= 1;

                    await _studentRepository.JoinRoom(roomId, studentId);
                    await _roomRepository.UpdateAvailableSpace(roomId, room);

                    return Ok("You have left the room");
                }
            }

            return BadRequest("You are not registered in any room");

        }


        //Student login 
        [HttpPost("student-login")]
        public async Task<ActionResult<Student>> Login([FromBody] LoginRequest loginRequest)
        {
            var student = await _studentRepository.GetStudentByUserName(loginRequest.UserName);
            if (student == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            if (!_authService.VerifyPasswordHash(loginRequest.Password, student.PasswordHash, student.PasswordSalt))
                return BadRequest(new { message = "UserName or password is incorrect" });

            string token = _authService.CreateStudentToken(student);
            student.AccessToken = token;

            await _studentRepository.UpdateStudentAccessToken(student.UserName, student);

            object studentDetails = new
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
                student.HallId,
                student.RoomId,
                student.Role,
                student.AccessToken,
                student.ProfileImageUrl
            };

            return Ok(studentDetails);
        }
    }
}