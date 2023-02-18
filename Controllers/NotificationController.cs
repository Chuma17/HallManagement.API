using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Implementations;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using Microsoft.AspNetCore.Mvc;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;

        public NotificationController(INotificationRepository notificationRepository, IHallRepository hallRepository,
                                        IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _hallRepository = hallRepository;
            _mapper = mapper;
        }

        //Add notification
        [HttpPost("add-notification")]
        public async Task<ActionResult<Notification>> AddNotification([FromBody] AddNotificationRequest request)
        {
            var hallExists = await _hallRepository.Exists(request.HallId);
            if (!hallExists)
            {
                return BadRequest("The specified hall ID is invalid.");
            }

            var notification = await _notificationRepository.CreateNotification(_mapper.Map<Notification>(request));
            return Ok(notification);
        }
        

    }
}
