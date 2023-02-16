using AutoMapper;
using HallManagementTest2.Repositories.Implementations;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet("get-studentDevices-in-hall/{studentDeviceId:guid}")]
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
            var studentDevice = await _studentDeviceRepository.AddStudentDeviceAsync(_mapper.Map<StudentDevice>(request));
            return Ok(studentDevice);
        }
    }
}
