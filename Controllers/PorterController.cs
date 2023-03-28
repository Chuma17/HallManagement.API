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
    [Route("api/[controller]")]
    [ApiController]
    public class PorterController : Controller
    {
        private readonly IPorterRepository _porterRepository;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly AuthService _authService;
        private readonly IHallRepository _hallRepository;

        public PorterController(IPorterRepository porterRepository, IMapper mapper,
                                IRoleService roleService, AuthService authService,
                                IHallRepository hallRepository)
        {
            _porterRepository = porterRepository;
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

        //Retrieving all the porters
        [HttpGet("get-Porters-by-gender"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<IActionResult> GetAllPorters()
        {
            var currentUserGender = User.FindFirstValue(ClaimTypes.Gender);

            var porters = await _porterRepository.GetPortersByGender(currentUserGender);

            var portersArray = new List<object>();

            foreach (var porter in porters)
            {
                var hall = await _hallRepository.GetHallAsync(porter.HallId);
                if (hall == null)
                {
                    hall.HallName = "Empty";
                }

                var portersList = new
                {
                    porter.PorterId,
                    porter.FirstName,
                    porter.LastName,
                    porter.Email,
                    hall.HallName
                };

                portersArray.Add(portersList);
            }

            return Ok(portersArray.ToArray());
        }

        //Retrieving a single porter
        [HttpGet("get-single-Porter/{porterId:guid}")]
        public async Task<IActionResult> GetPorterAsync([FromRoute] Guid porterId)
        {
            var porter = await _porterRepository.GetPorter(porterId);

            if (porter == null)
            {
                return NotFound();
            }

            var hall = await _hallRepository.GetHallAsync(porter.HallId);

            object porterDetails = new
            {
                porter.PorterId,
                porter.UserName,
                porter.FirstName,
                porter.LastName,
                porter.Email,
                hall.HallName,
                porter.Gender,
                porter.ProfileImageUrl,                
                porter.Role,
            };

            return Ok(porterDetails);
        }

        
        //Adding a porter
        [HttpPost("Porter-registration"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<ActionResult<Porter>> AddPorter([FromBody] AddPorterRequest request)
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

            var porter = _mapper.Map<Porter>(request);

            porter.PasswordHash = passwordHash;
            porter.PasswordSalt = passwordSalt;
            porter.Gender = currentUserGender;

            await _porterRepository.AddPorterAsync(porter);
            await _porterRepository.UpdatePorterPasswordHash(porter.PorterId, porter);            

            return Ok("Porter added successfully");
        }

        //Deleting a porter
        [HttpDelete("delete-porter/{porterId:guid}")]
        public async Task<IActionResult> DeletePorterAsync([FromRoute] Guid porterId)
        {
            if (await _porterRepository.Exists(porterId))
            {
                var porter = await _porterRepository.DeletePorterAsync(porterId);
                return Ok(_mapper.Map<Porter>(porter));
            }

            return NotFound();
        }

        //Updating a porter Record
        [HttpPut("update-Porter"), Authorize(Roles = "Porter")]
        public async Task<IActionResult> UpdatePorterAsync([FromBody] UpdatePorterRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                return Forbid();
            }

            if (await _porterRepository.Exists(currentUserIdGuid))
            {
                //Update Details
                var updatedPorter = await _porterRepository.UpdatePorter(currentUserIdGuid, _mapper.Map<Porter>(request));

                if (updatedPorter != null)
                {
                    return Ok("Account updated successfully");
                }
            }

            return NotFound();
        }

        //Porter login 
        [HttpPost("Porter-login")]
        public async Task<ActionResult<Porter>> Login([FromBody] LoginRequest loginRequest)
        {
            var porter = await _porterRepository.GetPorterByUserName(loginRequest.UserName);
            if (porter == null)
                return BadRequest(new { message = "Username is incorrect" });

            if (!_authService.VerifyPasswordHash(loginRequest.Password, porter.PasswordHash, porter.PasswordSalt))
                return BadRequest(new { message = "Password is incorrect" });

            string token = _authService.CreatePorterToken(porter);
            porter.AccessToken = token;

            var refreshToken = _authService.GenerateRefreshToken();
            _authService.SetPorterRefreshToken(refreshToken, porter, HttpContext);

            await _porterRepository.UpdatePorterToken(porter.UserName, porter);

            object porterDetails = new
            {
                porter.PorterId,
                porter.UserName,
                porter.Gender,
                porter.FirstName,
                porter.LastName,
                porter.Email,
                porter.HallId,
                porter.Role,
                porter.AccessToken,
                porter.RefreshToken,
                porter.ProfileImageUrl
            };

            return Ok(porterDetails);
        }

        //Porter refresh token
        [HttpPost("porter-refresh-token/{porterId:guid}")]
        public async Task<ActionResult<string>> RefreshToken([FromRoute] Guid porterId)
        {
            var porter = await _porterRepository.GetPorter(porterId);

            if (porter == null)
            {
                return NotFound();
            }

            var refreshToken = Request.Cookies["refreshToken"];

            if (!porter.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (porter.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            string token = _authService.CreatePorterToken(porter);
            porter.AccessToken = token;

            var newRefreshToken = _authService.GenerateRefreshToken();
            _authService.SetPorterRefreshToken(newRefreshToken, porter, HttpContext);

            await _porterRepository.UpdatePorterToken(porter.UserName, porter);

            return Ok(new { token });
        }

        [HttpPost("porter-logout"), Authorize]
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
