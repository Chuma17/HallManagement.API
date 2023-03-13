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
        private readonly IComplaintFormRepository _complaintForm;
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly IRoomRepository _roomRepository;

        public ComplaintFormController(IComplaintFormRepository complaintForm, IHallRepository hallRepository,
                                        IMapper mapper, IStudentRepository studentRepository,
                                        IRoomRepository roomRepository)
        {
            _complaintForm = complaintForm;
            _hallRepository = hallRepository;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _roomRepository = roomRepository;
        }

        //Add complaint form
        [HttpPost("add-complaintForm"), Authorize(Roles = "Student")]
        public async Task<ActionResult<Hall>> AddComplaintForm([FromBody] AddComplaintFormRequest request)
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

                if (student.RoomId == null)
                {
                    return BadRequest("You must be registered in a hall");
                }

                var hall = await _hallRepository.GetHallAsync(student.HallId);
                var room = await _roomRepository.GetRoomAsync(student.RoomId);

                var complaint = _mapper.Map<ComplaintForm>(request);
                complaint.HallId = hall.HallId;
                complaint.RoomNumber = room.RoomNumber;

                var complaintForm = await _complaintForm.AddComplaintFormAsync(complaint, hall.HallId);
                await _complaintForm.UpdateComplaintForm(complaintForm.ComplaintFormId, complaintForm);

                return Ok("Complaint has been submitted successfully!");
            }
            return BadRequest("User must be authenticated first");
        }
    }
}
