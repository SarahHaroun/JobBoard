using JobBoard.Domain.DTO.NotificationsDto;
using JobBoard.Services.NotificationsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // جلب كل الإشعارات للمستخدم
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        // إضافة إشعار جديد
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] CreateNotificationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _notificationService.AddNotificationAsync(userId, dto.Message, dto.Link);
            return Ok(new { message = "Notification added successfully." });
        }

        // تعليم إشعار واحد كمقروء
        [HttpPut("read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok(new { message = "Notification marked as read" });
        }

        // تعليم كل الإشعارات كمقروءة
        [HttpPut("user/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(string userId)
        {
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { message = "All notifications marked as read" });
        }

        // مسح إشعار واحد
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            await _notificationService.DeleteNotificationAsync(notificationId);
            return Ok(new { message = "Notification deleted" });
        }
    }
}