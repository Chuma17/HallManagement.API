using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HallManagementTest2.Controllers
{
    [Authorize(Roles = "ChiefHallAdmin")]
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

        //Retrieving a single hall type
        [HttpGet("get-halls-in-hallType/{hallTypeId:guid}")]
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
            var hallType = await _hallTypeRepository.AddHallTypeAsync(_mapper.Map<HallType>(request));
            return Ok(hallType);
        }

        //Delete hall type
        [HttpDelete("delete-hallType/{hallTypeId:guid}")]
        public async Task<IActionResult> DeleteHallTypeAsync([FromRoute] Guid hallTypeId)
        {
            if (await _hallTypeRepository.Exists(hallTypeId))
            {
                var hallType = await _hallTypeRepository.DeleteHallTypeAsync(hallTypeId);
                return Ok(_mapper.Map<HallType>(hallType));
            }

            return NotFound();
        }

        //Updating a Hall type Record
        [HttpPut("update-hallType/{hallTypeId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid hallTypeId, [FromBody] UpdateHallTypeRequest request)
        {
            if (await _hallTypeRepository.Exists(hallTypeId))
            {
                //Update Details
                var updatedHallType = await _hallTypeRepository.UpdateHallType(hallTypeId, _mapper.Map<HallType>(request));

                if (updatedHallType != null)
                {
                    return Ok(_mapper.Map<HallType>(updatedHallType));
                }
            }

            return NotFound();
        }
    }
}
