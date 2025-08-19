using JobBoard.Domain.DTO.NotificationsDto;
using JobBoard.Services.NotificationsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // for users only
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
    
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] CreateNotificationDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Message))
                return BadRequest("Invalid notification data.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found in token.");

            await _notificationService.AddNotificationAsync(userId, dto.Message, dto.Link);

            return Ok("Notification added successfully.");
        }

        // PUT: api/notifications/{id}/mark-as-read
        [HttpPut("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(notificationId);
                return Ok(new { message = "Notification marked as read" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }






    }
}
