using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using HallManagementTest2.Requests;
using HallManagementTest2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HallManagementTest2.Repositories.Interfaces;
using AutoMapper;
using System.Security.Claims;
using HallManagementTest2.Repositories.Implementations;

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
        [HttpGet("get-HallAdmins-by-gender"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<IActionResult> GetAllHallAdmins()
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);

            var hallAdmins = await _hallAdminRepository.GetHallAdminsByGender(currentUserGender);
            
            var hallAdminsArray = new List<object>();

            foreach (var hallAdmin in hallAdmins)
            {
                var hall = await _hallRepository.GetHallAsync(hallAdmin.HallId);
                if (hall == null)
                {
                    hall.HallName = "Empty";
                }

                var hallAdminsList = new
                {
                    hallAdmin.HallAdminId,
                    hallAdmin.FirstName,
                    hallAdmin.LastName,   
                    hallAdmin.Email,
                    hall.HallName
                };

                hallAdminsArray.Add(hallAdminsList);
            }

            return Ok(hallAdminsArray.ToArray());
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
                hallAdmin.Email,
                hallAdmin.Gender,
                hallAdmin.ProfileImageUrl,                
                hallAdmin.Role,
            };

            return Ok(hallAdminDetails);
        }

        //Get hall admin by hal
        [HttpGet("get-HallAdmin-by-hall/{hallId:guid}")]
        public async Task<IActionResult> GetHallAdminByHallAsync([FromRoute] Guid hallId)
        {
            var hallAdmin = await _hallAdminRepository.GetHallAdminByHall(hallId);
            if (hallAdmin == null)
            {
                return NotFound();
            }

            var hall = await _hallRepository.GetHallAsync(hallAdmin.HallId);
            if (hall == null)
            {
                hall.HallName = "Empty";
            }

            object hallAdminDetails = new
            {
                hallAdmin.HallAdminId,
                hallAdmin.UserName,
                hallAdmin.FirstName,
                hallAdmin.LastName,
                hallAdmin.Email,
                hall.HallName,
                hallAdmin.Gender,
                hallAdmin.ProfileImageUrl,                
                hallAdmin.Role,
            };

            return Ok(hallAdminDetails);
        }        

        //Adding a hall admin
        [HttpPost("HallAdmin-registration"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<ActionResult<HallAdmin>> AddHallAdmin([FromBody] AddHallAdminRequest request)
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);

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

            var hallAdmin = _mapper.Map<HallAdmin>(request);

            var hall = await _hallRepository.GetHallAsync(hallAdmin.HallId);
            hall.IsAssigned = true;

            hallAdmin.PasswordHash = passwordHash;
            hallAdmin.PasswordSalt = passwordSalt;
            hallAdmin.Gender = currentUserGender;

            await _hallAdminRepository.AddHallAdminAsync(hallAdmin);
            await _hallAdminRepository.UpdateHallAdminPasswordHash(hallAdmin.HallAdminId, hallAdmin);
            await _hallRepository.UpdateHallStatus(hall.HallId, hall);           

            return Ok("Account created successfully");
        }

        //Deleting a hall admin
        [HttpDelete("delete-HallAdmin/{hallAdminId:guid}"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<IActionResult> DeleteHallAdminAsync([FromRoute] Guid hallAdminId)
        {
            if (await _hallAdminRepository.Exists(hallAdminId))
            {
                var hallAdmin = await _hallAdminRepository.GetHallAdmin(hallAdminId);
                var hall = await _hallRepository.GetHallAsync(hallAdmin.HallId);

                hall.IsAssigned = false;

                await _hallRepository.UpdateHallStatus(hall.HallId, hall);
                await _hallAdminRepository.DeleteHallAdminAsync(hallAdminId);
                return Ok("This Hall admin account has been deleted");
            }

            return NotFound();
        }

        //Updating a hall admin Record
        [HttpPut("update-HallAdmin"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> UpdateHallAdminAsync([FromBody] UpdateHallAdminRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            if (await _hallAdminRepository.Exists(currentUserIdGuid))
            {                
                //Update Details
                var updatedHallAdmin = await _hallAdminRepository.UpdateHallAdmin(currentUserIdGuid, _mapper.Map<HallAdmin>(request));

                if (updatedHallAdmin != null)
                {                    
                    return Ok("Account Updated successfully");
                }
            }

            return NotFound();
        }

        //HallAdmin login 
        [HttpPost("HallAdmin-login")]
        public async Task<ActionResult<HallAdmin>> Login([FromBody] LoginRequest loginRequest)
        {
            var hallAdmin = await _hallAdminRepository.GetHallAdminByUserName(loginRequest.UserName);
            if (hallAdmin == null)
                return BadRequest(new { message = "Username is incorrect" });

            if (!_authService.VerifyPasswordHash(loginRequest.Password, hallAdmin.PasswordHash, hallAdmin.PasswordSalt))
                return BadRequest(new { message = "Password is incorrect" });

            string token = _authService.CreateHallAdminToken(hallAdmin);
            hallAdmin.AccessToken = token;

            var refreshToken = _authService.GenerateRefreshToken();
            _authService.SetHallAdminRefreshToken(refreshToken, hallAdmin, HttpContext);

            await _hallAdminRepository.UpdateHallAdminToken(hallAdmin.UserName, hallAdmin);

            var hall = await _hallRepository.GetHallAsync(hallAdmin.HallId);

            object hallAdminDetails = new
            {
                hallAdmin.HallAdminId,
                hallAdmin.UserName,
                hallAdmin.Gender,
                hallAdmin.FirstName,
                hallAdmin.LastName,
                HallName = hall?.HallName ?? "empty",
                hallAdmin.HallId,
                hallAdmin.Email,
                hallAdmin.Role,
                hallAdmin.AccessToken,
                hallAdmin.RefreshToken,
                hallAdmin.ProfileImageUrl
            };

            return Ok(hallAdminDetails);
        }

        //Hall admin refresh token
        [HttpPost("hallAdmin-refresh-token/{hallAdminId:guid}")]
        public async Task<ActionResult<string>> RefreshToken([FromRoute] Guid hallAdminId)
        {
            var hallAdmin = await _hallAdminRepository.GetHallAdmin(hallAdminId);

            if (hallAdmin == null)
            {
                return NotFound();
            }

            var refreshToken = Request.Cookies["refreshToken"];

            if (!hallAdmin.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (hallAdmin.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            string token = _authService.CreateHallAdminToken(hallAdmin);
            hallAdmin.AccessToken = token;

            var newRefreshToken = _authService.GenerateRefreshToken();
            _authService.SetHallAdminRefreshToken(newRefreshToken, hallAdmin, HttpContext);

            await _hallAdminRepository.UpdateHallAdminToken(hallAdmin.UserName, hallAdmin);

            return Ok(new { token });
        }

        [HttpPost("hallAdmin-logout"), Authorize]
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
