using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallTypeController : Controller
    {
        private readonly IHallTypeRepository _hallTypeRepository;
        private readonly IMapper _mapper;

        public HallTypeController(IHallTypeRepository hallTypeRepository, IMapper mapper)
        {
            _hallTypeRepository = hallTypeRepository;
            _mapper = mapper;
        }

        //Retrieving all hall types
        [HttpGet("get-all-hallTypes")]
        public async Task<IActionResult> GetAllHallTypes()
        {
            var hallTypes = await _hallTypeRepository.GetHallTypesAsync();

            return Ok(hallTypes);
        }

        //Retrieving halls in a hall type
        [HttpGet("get-halls-in-hallType/{hallTypeId:guid}")]
        public async Task<IActionResult> GetHallsInHallTypeAsync([FromRoute] Guid hallTypeId)
        {
            var hallType = await _hallTypeRepository.GetHallsInHallType(hallTypeId);

            if (hallType == null)
            {
                return NotFound();
            }

            return Ok(hallType.Halls);
        }

        //Retrieving a single hall type
        [HttpGet("get-hallType/{hallTypeId:guid}")]
        public async Task<IActionResult> GetHallTypeAsync([FromRoute] Guid hallTypeId)
        {
            var hallType = await _hallTypeRepository.GetHallTypeAsync(hallTypeId);

            if (hallType == null)
            {
                return NotFound();
            }

            return Ok(hallType);
        }

        //Add hall type
        [HttpPost("add-hallType")]
        public async Task<ActionResult<HallType>> AddHallType([FromBody] AddHallTypeRequest request)
        {
            await _hallTypeRepository.AddHallTypeAsync(_mapper.Map<HallType>(request));
            return Ok("Hall Type added successfully");
        }

        //Delete hall type
        [HttpDelete("delete-hallType/{hallTypeId:guid}"), Authorize(Roles = "ChiefHallAdmin")]
        public async Task<IActionResult> DeleteHallTypeAsync([FromRoute] Guid hallTypeId)
        {
            if (await _hallTypeRepository.Exists(hallTypeId))
            {

                var hallType = await _hallTypeRepository.GetHallTypeAsync(hallTypeId);
                if (hallType.HallCount == 0)
                {
                    await _hallTypeRepository.DeleteHallTypeAsync(hallTypeId);
                    return Ok("Hall Type deleted");
                }
                return BadRequest("There are still halls under this hall type");
            }

            return NotFound();
        }
    }
}
