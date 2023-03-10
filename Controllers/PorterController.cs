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
        [HttpGet("get-all-Porters")]
        public async Task<IActionResult> GetAllPorters()
        {
            var porters = await _porterRepository.GetPorters();

            return Ok(porters);
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
                porter.DateOfBirth,
                porter.Gender,
                porter.ProfileImageUrl,
                porter.Mobile,
                porter.Address,
                porter.State,
                porter.Role,
            };

            return Ok(porterDetails);
        }

        
        //Adding a porter
        [HttpPost("Porter-registration")]
        public async Task<ActionResult<Porter>> AddPorter([FromBody] AddPorterRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var porter = await _porterRepository.AddPorterAsync(_mapper.Map<Porter>(request));

            porter.PasswordHash = passwordHash;
            porter.PasswordSalt = passwordSalt;

            await _porterRepository.UpdatePorterPasswordHash(porter.PorterId, porter);

            object porterDetails = new
            {
                porter.PorterId,
                porter.UserName,
                porter.Gender,
                porter.FirstName,
                porter.LastName,
                porter.DateOfBirth,
                porter.Mobile,
                porter.Address,
                porter.State,
                porter.Role,
                porter.AccessToken,
                porter.ProfileImageUrl
            };

            return Ok(new { porterDetails });
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
        [HttpPut("update-Porter/{porterId:guid}"), Authorize(Roles = "Porter")]
        public async Task<IActionResult> UpdatePorterAsync([FromRoute] Guid porterId, [FromBody] UpdatePorterRequest request)
        {
            if (await _porterRepository.Exists(porterId))
            {
                //Update Details
                var updatedPorter = await _porterRepository.UpdatePorter(porterId, _mapper.Map<Porter>(request));

                if (updatedPorter != null)
                {
                    var UpdatedPorter = _mapper.Map<Porter>(updatedPorter);

                    object updatedPorterDetails = new
                    {
                        UpdatedPorter.UserName,
                        UpdatedPorter.Gender,
                        UpdatedPorter.FirstName,
                        UpdatedPorter.LastName,
                        UpdatedPorter.DateOfBirth,
                        UpdatedPorter.Mobile,
                        UpdatedPorter.Address,
                        UpdatedPorter.State,
                        UpdatedPorter.Role,
                    };

                    return Ok(updatedPorterDetails);
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
                return BadRequest(new { message = "Email or password is incorrect" });

            if (!_authService.VerifyPasswordHash(loginRequest.Password, porter.PasswordHash, porter.PasswordSalt))
                return BadRequest(new { message = "UserName or password is incorrect" });

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
                porter.DateOfBirth,
                porter.Mobile,
                porter.Address,
                porter.State,
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
