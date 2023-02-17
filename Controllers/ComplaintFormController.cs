using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintFormController : Controller
    {
        private readonly IComplaintFormRepository _complaintForm;
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;

        public ComplaintFormController(IComplaintFormRepository complaintForm, IHallRepository hallRepository,
                                        IMapper mapper)
        {
            _complaintForm = complaintForm;
            _hallRepository = hallRepository;
            _mapper = mapper;
        }

        //Add complaint form
        [HttpPost("add-complaintForm")]
        public async Task<ActionResult<Hall>> AddComplaintForm([FromBody] AddComplaintFormRequest request)
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

            var complaintForm = await _complaintForm.AddComplaintFormAsync(_mapper.Map<ComplaintForm>(request));

            return Ok(complaintForm);
        }
    }
}
