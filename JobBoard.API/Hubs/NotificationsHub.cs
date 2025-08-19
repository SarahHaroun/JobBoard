using Microsoft.AspNetCore.SignalR;

namespace JobBoard.API.Hubs
{
    public class NotificationsHub:Hub
    {
       
        public async Task SendNotification(string userId, string message, string? link = null)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message, link);
        }
    }
}
