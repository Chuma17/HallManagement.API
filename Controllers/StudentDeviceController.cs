using AutoMapper;
using HallManagementTest2.Repositories.Implementations;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HallManagementTest2.Requests;
using System.Data.Common;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentDeviceController : Controller
    {
        private readonly IStudentDeviceRepository _studentDeviceRepository;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public StudentDeviceController(IStudentDeviceRepository studentDeviceRepository, IMapper mapper
                                        , IStudentRepository studentRepository)
        {
            _studentDeviceRepository = studentDeviceRepository;
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        
        //Retrieving a single student device
        [HttpGet("get-single-studentDevice/{studentDeviceId:guid}")]
        public async Task<IActionResult> GetHallAsync([FromRoute] Guid studentDeviceId)
        {
            var studentDevice = await _studentDeviceRepository.GetStudentDeviceAsync(studentDeviceId);

            if (studentDevice == null)
            {
                return NotFound();
            }

            return Ok(studentDevice);

        }

        //Add student device
        [HttpPost("add-studentDevice"), Authorize(Roles = "Student")]
        public async Task<ActionResult<StudentDevice>> AddStudentDevice([FromBody] AddStudentDeviceRequest request)
        {
            var currentUserHallId = User.FindFirstValue(ClaimTypes.UserData);
            if (!Guid.TryParse(currentUserHallId, out Guid currentHallIdGuid))
            {
                return BadRequest("You must be registered in a hall first");
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            var currentUserMatricNo = User.FindFirstValue(ClaimTypes.SerialNumber);

            var studentDevice = _mapper.Map<StudentDevice>(request);
            studentDevice.StudentId = currentUserIdGuid;
            studentDevice.MatricNo = currentUserMatricNo;
            studentDevice.HallId = currentHallIdGuid;

            studentDevice = await _studentDeviceRepository.AddStudentDeviceAsync(currentHallIdGuid, studentDevice);            

            return Ok("The new device has been added successfully");
        }

        //Get StudentDevices by matric no
        [HttpPost("get-studentDevices-by-matricNo"), Authorize(Roles = "Porter,HallAdmin")]
        public async Task<IActionResult> GetStudentDeviceByMatricNo([FromBody] StudentDeviceRequest request)
        {
            var studentDevices = await _studentDeviceRepository.GetStudentDevicesByMatricNo(request.MatricNo);

            if (studentDevices == null)
            {
                return NotFound();
            }

            return Ok(studentDevices);
        }

        //Get pending StudentDevices
        [HttpPost("get-pending-studentDevices"), Authorize(Roles = "Porter")]
        public async Task<IActionResult> GetPendingStudentDevice()
        {
            var currentHallId = User.FindFirstValue(ClaimTypes.UserData);
            if (!Guid.TryParse(currentHallId, out Guid currentHallIdGuid))
            {
                return Forbid();
            }

            var studentDevices = await _studentDeviceRepository.GetPendingDevices(currentHallIdGuid);

            if (studentDevices == null)
            {
                return NotFound();
            }

            return Ok(studentDevices);
        }

        //Updating Student Device Status
        [HttpPut("update-device-status/{studentDeviceId:guid}"), Authorize(Roles = "Porter")]
        public async Task<IActionResult> UpdateDeviceStatus([FromRoute] Guid studentDeviceId)
        {
            if (await _studentDeviceRepository.Exists(studentDeviceId))
            {
                var studentDevice = await _studentDeviceRepository.GetStudentDeviceAsync(studentDeviceId);
                if (studentDevice.IsApproved)
                {
                    studentDevice.IsApproved = false;
                    await _studentDeviceRepository.UpdateDeviceStatus(studentDeviceId, studentDevice);
                    return Ok("Device has been unapproved");
                }

                else
                {
                    studentDevice.IsApproved = true;
                    await _studentDeviceRepository.UpdateDeviceStatus(studentDeviceId, studentDevice);
                    return Ok("Device has been approved");
                }
            }

            return NotFound();
        }

    }
}
