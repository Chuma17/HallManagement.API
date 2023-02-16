﻿using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using HallManagementTest2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                chiefHallAdmin.DateOfBirth,
                chiefHallAdmin.Gender,
                chiefHallAdmin.ProfileImageUrl,
                chiefHallAdmin.Mobile,
                chiefHallAdmin.Address,
                chiefHallAdmin.State,
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

            var chiefHallAdmin = await _chiefHallAdminRepository.AddChiefHallAdminAsync(_mapper.Map<ChiefHallAdmin>(request));

            chiefHallAdmin.PasswordHash = passwordHash;
            chiefHallAdmin.PasswordSalt = passwordSalt;

            await _chiefHallAdminRepository.UpdateChiefHallAdminPasswordHash(chiefHallAdmin.ChiefHallAdminId, chiefHallAdmin);

            object chiefHallAdminDetails = new
            {
                chiefHallAdmin.ChiefHallAdminId,
                chiefHallAdmin.UserName,
                chiefHallAdmin.Gender,
                chiefHallAdmin.FirstName,
                chiefHallAdmin.LastName,
                chiefHallAdmin.DateOfBirth,
                chiefHallAdmin.Mobile,
                chiefHallAdmin.Address,
                chiefHallAdmin.State,
                chiefHallAdmin.Role,
                chiefHallAdmin.AccessToken,
                chiefHallAdmin.ProfileImageUrl
            };

            return Ok(new { chiefHallAdminDetails });
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
        [HttpPut("update-chiefHallAdmin/{chiefHallAdminId:guid}")]
        public async Task<IActionResult> UpdateChiefHallAdminAsync([FromRoute] Guid chiefHallAdminId, [FromBody] UpdateChiefHallAdminRequest request)
        {
            if (await _chiefHallAdminRepository.Exists(chiefHallAdminId))
            {
                //Update Details
                var updatedChiefHallAdmin = await _chiefHallAdminRepository.UpdateChiefHallAdmin(chiefHallAdminId, _mapper.Map<ChiefHallAdmin>(request));

                if (updatedChiefHallAdmin != null)
                {
                    var UpdatedChiefHallAdmin = _mapper.Map<ChiefHallAdmin>(updatedChiefHallAdmin);

                    object updatedchiefHallAdminDetails = new
                    {
                        UpdatedChiefHallAdmin.UserName,
                        UpdatedChiefHallAdmin.Gender,
                        UpdatedChiefHallAdmin.FirstName,
                        UpdatedChiefHallAdmin.LastName,
                        UpdatedChiefHallAdmin.DateOfBirth,
                        UpdatedChiefHallAdmin.Mobile,
                        UpdatedChiefHallAdmin.Address,
                        UpdatedChiefHallAdmin.State,
                        UpdatedChiefHallAdmin.Role,
                    };

                    return Ok(updatedchiefHallAdminDetails);
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
                return BadRequest(new { message = "Email or password is incorrect" });

            if (!_authService.VerifyPasswordHash(loginRequest.Password, chiefHallAdmin.PasswordHash, chiefHallAdmin.PasswordSalt))
                return BadRequest(new { message = "UserName or password is incorrect" });

            string token = _authService.CreateChiefHallAdminToken(chiefHallAdmin);
            chiefHallAdmin.AccessToken = token;

            await _chiefHallAdminRepository.UpdateChiefHallAdminAccessToken(chiefHallAdmin.UserName, chiefHallAdmin);

            object chiefHallAdminDetails = new
            {
                chiefHallAdmin.ChiefHallAdminId,
                chiefHallAdmin.UserName,
                chiefHallAdmin.Gender,
                chiefHallAdmin.FirstName,
                chiefHallAdmin.LastName,
                chiefHallAdmin.DateOfBirth,
                chiefHallAdmin.Mobile,
                chiefHallAdmin.Address,
                chiefHallAdmin.State,
                chiefHallAdmin.Role,
                chiefHallAdmin.AccessToken,
                chiefHallAdmin.ProfileImageUrl
            };

            return Ok(chiefHallAdminDetails);
        }
    }
}
