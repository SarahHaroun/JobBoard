using JobBoard.Domain.Services.Contract;
using Microsoft.AspNetCore.SignalR;

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
			await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message, link);
		}

		public async Task SendNotificationUpdateAsync(string userId, object updateData)
		{
			await _hubContext.Clients.Group($"User_{userId}").SendAsync("NotificationUpdate", updateData);
		}
	}

}
