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

        public async Task SendNotificationAsync(string userId, string message, string? link = null, int id = 0)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message, link, id);
        }
    }

}
