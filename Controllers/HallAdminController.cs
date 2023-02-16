using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using HallManagementTest2.Requests;
using HallManagementTest2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HallManagementTest2.Repositories.Interfaces;
using AutoMapper;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallAdminController : Controller
    {
        private readonly IHallAdminRepository _hallAdminRepository;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly AuthService _authService;
        private readonly IHallRepository _hallRepository;

        public HallAdminController(IHallAdminRepository hallAdminRepository, IMapper mapper,
                                    IRoleService roleService, AuthService authService, IHallRepository hallRepository)
        {
            _hallAdminRepository = hallAdminRepository;
            _mapper = mapper;
            _roleService = roleService;
            _authService = authService;
            _hallRepository = hallRepository;
        }

        //Getting the role of the user
        [HttpGet("get-roles"), Authorize]
        public ActionResult<object> GetMe()
        {
            var role = _roleService.GetRole();
            return Ok(new { role });

        }

        //Retrieving all the hall admins
        [HttpGet("get-all-HallAdmins")]
        public async Task<IActionResult> GetAllHallAdmins()
        {
            var hallAdmins = await _hallAdminRepository.GetHallAdmins();

            return Ok(hallAdmins);
        }

        //Retrieving a single hall admin
        [HttpGet("get-single-HallAdmin/{hallAdminId:guid}")]
        public async Task<IActionResult> GetHallAdminAsync([FromRoute] Guid hallAdminId)
        {
            var hallAdmin = await _hallAdminRepository.GetHallAdmin(hallAdminId);

            if (hallAdmin == null)
            {
                return NotFound();
            }

            object hallAdminDetails = new
            {
                hallAdmin.HallAdminId,
                hallAdmin.UserName,
                hallAdmin.FirstName,
                hallAdmin.LastName,
                hallAdmin.DateOfBirth,
                hallAdmin.Gender,
                hallAdmin.ProfileImageUrl,
                hallAdmin.Mobile,
                hallAdmin.Address,
                hallAdmin.State,
                hallAdmin.Role,
            };

            return Ok(hallAdminDetails);
        }

        [HttpGet("get-HallAdmin-by-hall/{hallId:guid}")]
        public async Task<IActionResult> GetHallAdminByHallAsync([FromRoute] Guid hallId)
        {
            var hallAdmin = await _hallAdminRepository.GetHallAdminByHall(hallId);

            if (hallAdmin == null)
            {
                return NotFound();
            }

            object hallAdminDetails = new
            {
                hallAdmin.HallAdminId,
                hallAdmin.UserName,
                hallAdmin.FirstName,
                hallAdmin.LastName,
                hallAdmin.DateOfBirth,
                hallAdmin.Gender,
                hallAdmin.ProfileImageUrl,
                hallAdmin.Mobile,
                hallAdmin.Address,
                hallAdmin.State,
                hallAdmin.Role,
            };

            return Ok(hallAdminDetails);
        }

        //Adding a hall admin
        [HttpPost("HallAdmin-registration")]
        public async Task<ActionResult<HallAdmin>> AddHallAdmin([FromBody] AddHallAdminRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var hallExists = await _hallRepository.Exists(request.HallId);
            if (!hallExists)
            {
                return BadRequest("The specified hall ID is invalid.");
            }

            _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var hallAdmin = await _hallAdminRepository.AddHallAdminAsync(_mapper.Map<HallAdmin>(request));

            hallAdmin.PasswordHash = passwordHash;
            hallAdmin.PasswordSalt = passwordSalt;

            await _hallAdminRepository.UpdateHallAdminPasswordHash(hallAdmin.HallAdminId, hallAdmin);

            object hallAdminDetails = new
            {
                hallAdmin.HallAdminId,
                hallAdmin.UserName,
                hallAdmin.Gender,
                hallAdmin.FirstName,
                hallAdmin.LastName,
                hallAdmin.DateOfBirth,
                hallAdmin.Mobile,
                hallAdmin.Address,
                hallAdmin.State,
                hallAdmin.Role,
                hallAdmin.AccessToken,
                hallAdmin.ProfileImageUrl
            };

            return Ok(new { hallAdminDetails });
        }

        //Deleting a hall admin
        [HttpDelete("delete-HallAdmin/{hallAdminId:guid}")]
        public async Task<IActionResult> DeleteHallAdminAsync([FromRoute] Guid hallAdminId)
        {
            if (await _hallAdminRepository.Exists(hallAdminId))
            {
                var hallAdmin = await _hallAdminRepository.DeleteHallAdminAsync(hallAdminId);
                return Ok(_mapper.Map<HallAdmin>(hallAdmin));
            }

            return NotFound();
        }

        //Updating a hall admin Record
        [HttpPut("update-HallAdmin/{hallAdminId:guid}"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> UpdateHallAdminAsync([FromRoute] Guid hallAdminId, [FromBody] UpdateHallAdminRequest request)
        {
            if (await _hallAdminRepository.Exists(hallAdminId))
            {                
                //Update Details
                var updatedHallAdmin = await _hallAdminRepository.UpdateHallAdmin(hallAdminId, _mapper.Map<HallAdmin>(request));

                if (updatedHallAdmin != null)
                {
                    var UpdatedHallAdmin = _mapper.Map<HallAdmin>(updatedHallAdmin);

                    object updatedHallAdminDetails = new
                    {
                        UpdatedHallAdmin.UserName,
                        UpdatedHallAdmin.Gender,
                        UpdatedHallAdmin.FirstName,
                        UpdatedHallAdmin.LastName,
                        UpdatedHallAdmin.DateOfBirth,
                        UpdatedHallAdmin.Mobile,
                        UpdatedHallAdmin.Address,
                        UpdatedHallAdmin.State,
                        UpdatedHallAdmin.Role,
                    };

                    return Ok(updatedHallAdminDetails);
                }
            }

            return NotFound();
        }

        //HallAdmin login 
        [HttpPost("HallAdmin-login"), AllowAnonymous]
        public async Task<ActionResult<HallAdmin>> Login([FromBody] LoginRequest loginRequest)
        {
            var hallAdmin = await _hallAdminRepository.GetHallAdminByUserName(loginRequest.UserName);
            if (hallAdmin == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            if (!_authService.VerifyPasswordHash(loginRequest.Password, hallAdmin.PasswordHash, hallAdmin.PasswordSalt))
                return BadRequest(new { message = "UserName or password is incorrect" });

            string token = _authService.CreateHallAdminToken(hallAdmin);
            hallAdmin.AccessToken = token;

            await _hallAdminRepository.UpdateHallAdminAccessToken(hallAdmin.UserName, hallAdmin);

            object hallAdminDetails = new
            {
                hallAdmin.HallAdminId,
                hallAdmin.UserName,
                hallAdmin.Gender,
                hallAdmin.FirstName,
                hallAdmin.LastName,
                hallAdmin.DateOfBirth,
                hallAdmin.Mobile,
                hallAdmin.Address,
                hallAdmin.State,
                hallAdmin.Role,
                hallAdmin.AccessToken,
                hallAdmin.ProfileImageUrl
            };

            return Ok(hallAdminDetails);
        }
    }
}
