using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Implementations;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using HallManagementTest2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HallManagementTest2.Controllers
{
    [Authorize(Roles = "ChiefHallAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChiefHallAdminController : Controller
    {
        private readonly IChiefHallAdminRepository _chiefHallAdminRepository;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly AuthService _authService;

        public ChiefHallAdminController(IChiefHallAdminRepository chiefHallAdminRepository,
                                        IMapper mapper, IRoleService roleService,
                                        AuthService authService)
        {
            _chiefHallAdminRepository = chiefHallAdminRepository;
            _mapper = mapper;
            _roleService = roleService;
            _authService = authService;
        }

        //Getting the role of the user
        [HttpGet("get-roles")]
        public ActionResult<object> GetMe()
        {
            var role = _roleService.GetRole();
            return Ok(new { role });

        }

        //Retrieving all the chief hall admins
        [HttpGet("get-all-chiefHallAdmins")]
        public async Task<IActionResult> GetAllChiefHallAdmins()
        {
            var chiefHallAdmins = await _chiefHallAdminRepository.GetChiefHallAdmins();

            return Ok(chiefHallAdmins);
        }

        //Retrieving a single chief hall admin
        [HttpGet("get-single-chiefHallAdmin/{chiefHallAdminId:guid}")]
        public async Task<IActionResult> GetchiefHallAdminAsync([FromRoute] Guid chiefHallAdminId)
        {
            var chiefHallAdmin = await _chiefHallAdminRepository.GetChiefHallAdmin(chiefHallAdminId);

            if (chiefHallAdmin == null)
            {
                return NotFound();
            }

            object chiefHallAdminDetails = new
            {
                chiefHallAdmin.ChiefHallAdminId,
                chiefHallAdmin.UserName,
                chiefHallAdmin.FirstName,
                chiefHallAdmin.LastName,
                chiefHallAdmin.Email,
                chiefHallAdmin.Gender,
                chiefHallAdmin.ProfileImageUrl,
                chiefHallAdmin.Role,
            };

            return Ok(chiefHallAdminDetails);
        }

        //Adding a chief hall admin
        [HttpPost("chiefHallAdmin-registration")]
        public async Task<ActionResult<ChiefHallAdmin>> AddChiefHallAdmin([FromBody] AddChiefHallAdminRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var chiefHallAdmin = _mapper.Map<ChiefHallAdmin>(request);

            chiefHallAdmin.PasswordHash = passwordHash;
            chiefHallAdmin.PasswordSalt = passwordSalt;

            await _chiefHallAdminRepository.AddChiefHallAdminAsync(chiefHallAdmin);

            await _chiefHallAdminRepository.UpdateChiefHallAdminPasswordHash(chiefHallAdmin.ChiefHallAdminId, chiefHallAdmin);            

            return Ok("Account Created successfully");
        }

        //Deleting a chief hall admin
        [HttpDelete("delete-chiefHallAdmin/{chiefHallAdminId:guid}")]
        public async Task<IActionResult> DeleteChiefHallAdminAsync([FromRoute] Guid chiefHallAdminId)
        {
            if (await _chiefHallAdminRepository.Exists(chiefHallAdminId))
            {
                var chiefHallAdmin = await _chiefHallAdminRepository.DeleteChiefHallAdminAsync(chiefHallAdminId);
                return Ok(_mapper.Map<ChiefHallAdmin>(chiefHallAdmin));
            }

            return NotFound();
        }

        //Updating a chief hall admin Record
        [HttpPut("update-chiefHallAdmin")]
        public async Task<IActionResult> UpdateChiefHallAdminAsync([FromBody] UpdateChiefHallAdminRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            if (await _chiefHallAdminRepository.Exists(currentUserIdGuid))
            {
                //Update Details
                var updatedChiefHallAdmin = await _chiefHallAdminRepository.UpdateChiefHallAdmin(currentUserIdGuid, _mapper.Map<ChiefHallAdmin>(request));

                if (updatedChiefHallAdmin != null)
                {                    
                    return Ok("Account Updated Successfully");
                }
            }

            return NotFound();
        }

        //ChiefHallAdmin login 
        [HttpPost("chiefHallAdmin-login"), AllowAnonymous]
        public async Task<ActionResult<ChiefHallAdmin>> Login([FromBody] LoginRequest loginRequest)
        {
            var chiefHallAdmin = await _chiefHallAdminRepository.GetChiefHallAdminByUserName(loginRequest.UserName);
            if (chiefHallAdmin == null)
                return BadRequest(new { message = "Username is incorrect" });

            if (!_authService.VerifyPasswordHash(loginRequest.Password, chiefHallAdmin.PasswordHash, chiefHallAdmin.PasswordSalt))
                return BadRequest(new { message = "Password is incorrect" });

            string token = _authService.CreateChiefHallAdminToken(chiefHallAdmin);
            chiefHallAdmin.AccessToken = token;

            var refreshToken = _authService.GenerateRefreshToken();
            _authService.SetCHiefHallAdminRefreshToken(refreshToken, chiefHallAdmin, HttpContext);

            await _chiefHallAdminRepository.UpdateChiefHallAdminToken(chiefHallAdmin.UserName, chiefHallAdmin);

            object chiefHallAdminDetails = new
            {
                chiefHallAdmin.ChiefHallAdminId, 
                chiefHallAdmin.UserName, 
                chiefHallAdmin.Gender,
                chiefHallAdmin.FirstName, 
                chiefHallAdmin.LastName, 
                chiefHallAdmin.Role, 
                chiefHallAdmin.AccessToken, 
                chiefHallAdmin.RefreshToken,
                chiefHallAdmin.ProfileImageUrl
            };

            return Ok(chiefHallAdminDetails);
        }

        //Cheif Hall admin refresh token
        [HttpPost("chiefHallAdmin-refresh-token/{chiefHallAdminId:guid}"), AllowAnonymous]
        public async Task<ActionResult<string>> RefreshToken([FromRoute] Guid chiefHallAdminId)
        {
            var chiefHallAdmin = await _chiefHallAdminRepository.GetChiefHallAdmin(chiefHallAdminId);

            if (chiefHallAdmin == null)
            {
                return NotFound();
            }

            var refreshToken = Request.Cookies["refreshToken"];

            if (!chiefHallAdmin.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (chiefHallAdmin.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            string token = _authService.CreateChiefHallAdminToken(chiefHallAdmin);
            chiefHallAdmin.AccessToken = token;

            var newRefreshToken = _authService.GenerateRefreshToken();
            _authService.SetCHiefHallAdminRefreshToken(newRefreshToken, chiefHallAdmin, HttpContext);

            await _chiefHallAdminRepository.UpdateChiefHallAdminToken(chiefHallAdmin.UserName, chiefHallAdmin);

            return Ok(new { token });
        }

        [HttpPost("chiefHallAdmin-logout")]
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
