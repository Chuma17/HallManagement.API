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
    public class ExitPassController : Controller
    {
        private readonly IExitPassRepository _exitPassRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly IHallAdminRepository _hallAdminRepository;
        private readonly IPorterRepository _porterRepository;

        public ExitPassController(IExitPassRepository exitPassRepository, IHallRepository hallRepository,
                                    IStudentRepository studentRepository, IMapper mapper,
                                    IHallAdminRepository hallAdminRepository, IPorterRepository porterRepository)
        {
            _exitPassRepository = exitPassRepository;
            _hallRepository = hallRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _hallAdminRepository = hallAdminRepository;
            _porterRepository = porterRepository;
        }

        //all exit passes
        [HttpGet("get-all-exitPasses")]
        public async Task<IActionResult> GetExitPasses()
        {
            var exitPasses = await _exitPassRepository.GetExitPassesAsync();
            return Ok(exitPasses);
        }

        //add exit pass
        [HttpPost("add-exitPass"), Authorize(Roles = "Student")]
        public async Task<IActionResult> AddExitPass([FromBody] AddExitPassRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                if (request == null)
                {
                    return BadRequest();
                }

                var student = await _studentRepository.GetStudentAsync(currentUserIdGuid);
                var hall = await _hallRepository.GetHallAsync(student.HallId);

                if (student != null)
                {
                    var exit = _mapper.Map<ExitPass>(request);
                    exit.StudentId = currentUserIdGuid;
                    exit.HallId = hall.HallId;

                    if (exit.DateOfReturn < exit.DateOfExit)
                    {
                        return BadRequest("Your date of return must be after your date of exit");
                    }
                    if (exit.DateOfExit < DateTime.Now || exit.DateOfReturn < DateTime.Now)
                    {
                        return BadRequest("Your date of return and exit cannot be before today");
                    }

                    var exitPass = await _exitPassRepository.AddExitPassAsync(exit, hall.HallId, student.StudentId);
                    return Ok(exitPass);
                }

                else
                {
                    return NotFound();
                }

            }

            return Forbid();

        }

        //single exit pass
        [HttpGet("get-single-exitPass/{exitPassId:guid}")]
        public async Task<IActionResult> GetExitPass([FromRoute] Guid exitPassId)
        {
            var exitPass = await _exitPassRepository.GetExitPass(exitPassId);
            if (exitPass == null)
            {
                return NotFound();
            }

            return Ok(exitPass);
        }

        //search for exit pass by matric no
        [HttpPost("get-exitPasses-by-matricNo"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> GetExitPassForStudent([FromBody] string matricNo)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                var hallAdmin = await _hallAdminRepository.GetHallAdmin(currentUserIdGuid);
                if (hallAdmin != null)
                {
                    var exitPasses = await _exitPassRepository.GetExitPassesForStudent(matricNo, hallAdmin.HallId);
                    if (exitPasses == null)
                    {
                        return NotFound();
                    }

                    return Ok(exitPasses);
                }                                           
            }

            return Forbid();
        }

        //filter all approved exit pass
        [HttpGet("get-approved-exitPasses"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> GetApprovedExitPass()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                var hallAdmin = await _hallAdminRepository.GetHallAdmin(currentUserIdGuid);
                if (hallAdmin != null)
                {
                    var exitPasses = await _exitPassRepository.GetApprovedExitPassesAsync(hallAdmin.HallId);
                    if (exitPasses == null)
                    {
                        return NotFound();
                    }

                    return Ok(exitPasses);
                }                
            }

            return Forbid();
        }

        //get valid and pending exit pass
        [HttpGet("get-pending-exitPasses"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> GetPendingExitPass()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                var hallAdmin = await _hallAdminRepository.GetHallAdmin(currentUserIdGuid);
                if (hallAdmin != null)
                {
                    var exitPasses = await _exitPassRepository.GetPendingExitPassesAsync(hallAdmin.HallId);
                    if (exitPasses == null)
                    {
                        return NotFound();
                    }

                    return Ok(exitPasses);
                }
            }

            return Forbid();
        }

        //get students due to return the current day
        [HttpGet("get-students-due"), Authorize(Roles = "HallAdmin,Porter")]
        public async Task<IActionResult> GetDueExitPass()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                var hallAdmin = await _hallAdminRepository.GetHallAdmin(currentUserIdGuid);
                if (hallAdmin != null)
                {
                    var exitPasses1 = await _exitPassRepository.GetStudentsDueToReturn(hallAdmin.HallId);
                    if (exitPasses1 == null)
                    {
                        return NotFound();
                    }

                    return Ok(exitPasses1);
                }

                var porter = await _porterRepository.GetPorter(currentUserIdGuid);
                if (porter != null)
                {
                    var exitPasses = await _exitPassRepository.GetStudentsDueToReturn(porter.HallId);
                    if (exitPasses == null)
                    {
                        return NotFound();
                    }

                    return Ok(exitPasses);
                }
            }

            return Forbid();
        }

        //get students who have exceeded their stay
        [HttpGet("get-students-over-due"), Authorize(Roles = "HallAdmin,Porter")]
        public async Task<IActionResult> GetOverDueExitPass()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                var hallAdmin = await _hallAdminRepository.GetHallAdmin(currentUserIdGuid);
                if (hallAdmin != null)
                {
                    var exitPasses1 = await _exitPassRepository.GetStudentsOverDueToReturn(hallAdmin.HallId);
                    if (exitPasses1 == null)
                    {
                        return NotFound();
                    }

                    return Ok(exitPasses1);
                }

                var porter = await _porterRepository.GetPorter(currentUserIdGuid);
                if (porter != null)
                {
                    var exitPasses = await _exitPassRepository.GetStudentsOverDueToReturn(porter.HallId);
                    if (exitPasses == null)
                    {
                        return NotFound();
                    }

                    return Ok(exitPasses);
                }
            }

            return Forbid();
        }

        //Approve exit pass
        [HttpPut("approve-exitPass/{exitPassId:guid}"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> ApproveExitPass([FromRoute] Guid exitPassId)
        {
            var exitPass = await _exitPassRepository.GetExitPass(exitPassId);
            if (exitPass == null)
            {
                return NotFound();
            }

            await _exitPassRepository.ApproveExitPass(exitPassId);
            return Ok("Exit Pass has been Approved!");
        }

        //Update student status
        [HttpPut("upadet-hasReturned-status/{exitPassId:guid}"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> UpdateStudentStatus([FromRoute] Guid exitPassId)
        {
            var exitPass = await _exitPassRepository.GetExitPass(exitPassId);
            if (exitPass == null)
            {
                return NotFound();
            }

            await _exitPassRepository.UpdateStudentExitPass(exitPassId);
            return Ok("Student status has been updated");
        }
    }
}
