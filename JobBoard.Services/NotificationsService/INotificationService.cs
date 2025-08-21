using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.NotificationsDto;
using JobBoard.Domain.Entities;

namespace JobBoard.Services.NotificationsService
{
    public interface INotificationService
    {
        /// <summary>
        /// Adds a new notification for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to whom the notification is directed.</param>
        /// <param name="message">The message content of the notification.</param>
        /// <param name="link">An optional link for more details.</param>
        Task AddNotificationAsync(string userId, string message, string? link = null);
        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to mark as read.</param>
        Task MarkAsReadAsync(int notificationId);
        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose notifications are to be retrieved.</param>
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId);
        Task MarkAllAsReadAsync(string userId);
        Task DeleteNotificationAsync(int notificationId);



    }
}
