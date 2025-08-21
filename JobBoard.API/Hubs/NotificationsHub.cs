using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace JobBoard.API.Hubs
{
    [Authorize] 
    public class NotificationsHub : Hub
    {
        
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
                Console.WriteLine($"User {userId} connected with connection ID: {Context.ConnectionId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
                Console.WriteLine($"User {userId} disconnected");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotificationToUser(string userId, string message, string? link = null)
        {
            await Clients.Group($"User_{userId}").SendAsync("ReceiveNotification", message, link);
        }

        public async Task SendNotificationToAll(string message, string? link = null)
        {
            await Clients.All.SendAsync("ReceiveNotification", message, link);
        }


    }
}