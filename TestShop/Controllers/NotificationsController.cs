using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestShop.Application.DTOs.Common;
using TestShop.Application.DTOs.Notifications;
using TestShop.Application.Interfaces;

namespace TestShop.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService) { _notificationService = notificationService; }
        private int UserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet("latest")]
        public async Task<ActionResult<Result<NotificationDto>>> Latest(CancellationToken cancellationToken)
        {
            var notification = await _notificationService.GetLatestForUserAsync(UserId(), cancellationToken);
            if (notification is null) 
                return Ok(Result<NotificationDto>.Fail("NONE", "No notifications yet."));
            return Ok(Result<NotificationDto>.Ok(notification));
        }
    }
}
