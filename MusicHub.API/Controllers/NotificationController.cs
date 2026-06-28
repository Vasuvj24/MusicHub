using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.Application.Services;
using System.Security.Claims;

namespace MusicHub.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController
    : ControllerBase
    {
        private readonly NotificationService _service;

        public NotificationController(
            NotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult>
            GetNotifications()
        {
            var userId = Guid.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!.Value);

            return Ok(
                await _service
                    .GetNotificationsAsync(userId));
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult>
            GetUnreadCount()
        {
            var userId = Guid.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!.Value);

            return Ok(new
            {
                Count = await _service
                    .GetUnreadCountAsync(userId)
            });
        }

        [HttpPut("{notificationId:guid}/read")]
        public async Task<IActionResult>
            MarkRead(Guid id)
        {
            await _service.MarkAsReadAsync(id);

            return Ok();
        }
    }
}
