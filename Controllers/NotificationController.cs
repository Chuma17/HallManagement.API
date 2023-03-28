using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Implementations;
using HallManagementTest2.Repositories.Interfaces;
using HallManagementTest2.Requests.Add;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HallManagementTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;
        private readonly IHallAdminRepository _hallAdminRepository;
        private readonly IPorterRepository _porterRepository;

        public NotificationController(INotificationRepository notificationRepository, IHallRepository hallRepository,
                                        IMapper mapper, IHallAdminRepository hallAdminRepository, IPorterRepository porterRepository)
        {
            _notificationRepository = notificationRepository;
            _hallRepository = hallRepository;
            _mapper = mapper;
            _hallAdminRepository = hallAdminRepository;
            _porterRepository = porterRepository;
        }

        //Add notification
        [HttpPost("add-notification"), Authorize(Roles = "HallAdmin,Porter")]
        public async Task<ActionResult<Notification>> AddNotification([FromBody] AddNotificationRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(currentUserId, out Guid currentUserIdGuid))
            {
                if (request == null)
                {
                    return BadRequest();
                }

                var hallAdmin = await _hallAdminRepository.GetHallAdmin(currentUserIdGuid);
                if (hallAdmin != null)
                {
                    var hallExists = await _hallRepository.Exists(hallAdmin.HallId);
                    if (!hallExists)
                    {
                        return BadRequest("The specified hall ID is invalid.");
                    }
                    var notif = _mapper.Map<Notification>(request);
                    notif.HallId = hallAdmin.HallId;

                    var notification = await _notificationRepository.CreateNotification(notif, notif.HallId);                    

                    await _notificationRepository.UpdateNotification(notification.NotiFicationId, notification);
                    return Ok("Notification Posted succesfully!");

                }

                var porter = await _porterRepository.GetPorter(currentUserIdGuid);
                if (porter != null)
                {
                    var hallExists = await _hallRepository.Exists(porter.HallId);
                    if (!hallExists)
                    {
                        return BadRequest("The specified hall ID is invalid.");
                    }

                    var notif = _mapper.Map<Notification>(request);
                    notif.HallId = porter.HallId;

                    var notification = await _notificationRepository.CreateNotification(notif, notif.HallId);

                    await _notificationRepository.UpdateNotification(notification.NotiFicationId, notification);
                    return Ok("Notification Posted succesfully!");

                }
            }

            return BadRequest("User must be authenticated first");
        }

        //Get Notifications in hall
        [HttpGet("get-notifications-in-hall/{hallId:guid}"), Authorize(Roles = "HallAdmin")]
        public async Task<IActionResult> GetNotificationsInHall([FromRoute] Guid hallId)
        {
            var hall = await _hallRepository.GetHallAsync(hallId);
            if (hall == null)
            {
                return BadRequest("Hall does not exist");
            }

            var notifications = await _notificationRepository.GetNotificationInHall(hallId);

            return Ok(notifications);
        }
    }
}
