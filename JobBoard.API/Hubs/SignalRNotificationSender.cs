using JobBoard.Domain.Entities;
using JobBoard.Domain.Services.Contract;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.API.Hubs
{
	public class SignalRNotificationSender : INotificationSender
	{
		private readonly IHubContext<NotificationsHub> _hubContext;
		public SignalRNotificationSender(IHubContext<NotificationsHub> hubContext)
		{
			_hubContext = hubContext;
		}

        public async Task SendNotificationAsync(string userId, string message, string? link = null)
        {
            Console.WriteLine($"Sending notification to User_{userId} with message: {message}"); // Add this line
            await _hubContext.Clients.Group($"User_{userId}").SendAsync("ReceiveNotification", message, link);
        }

        public async Task SendNotificationUpdateAsync(string userId, object updateData)
		{
			await _hubContext.Clients.Group($"User_{userId}").SendAsync("NotificationUpdate", updateData);
		}
	}

}