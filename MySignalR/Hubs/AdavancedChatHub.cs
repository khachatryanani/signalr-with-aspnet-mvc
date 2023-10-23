using Microsoft.AspNetCore.SignalR;
using MySignalR.Data;
using System.Security.Claims;

namespace MySignalR.Hubs
{
    public class AdavancedChatHub : Hub
    {
        public readonly ApplicationDbContext _db;
        public AdavancedChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendRoomAdded(int roomId, string roomName)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userName = _db.Users.FirstOrDefault(u => u.Id.Equals(userId)).UserName;

            await Clients.All.SendAsync("RoomAdded", roomId, roomName, userId, userName);
        }

        public async Task SendRoomDeleted(int deleted, int selected, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;

            await Clients.All.SendAsync("RoomDeleted", deleted, selected, roomName, userName);
        }

        public async Task SendPublicMessage(int roomId, string message, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;

            await Clients.All.SendAsync("PublicMessage", roomId, UserId, userName, message, roomName);
        }

        public async Task SendPrivateMessage(string receiverId, string message, string receiverName)
        {
            var senderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _db.Users.FirstOrDefault(u => u.Id == senderId).UserName;

            var users = new string[] { senderId, receiverId };

            await Clients.Users(users).SendAsync("PrivateMessage", senderId, senderName, receiverId, message, Guid.NewGuid(), receiverName);
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                string userName = _db.Users.FirstOrDefault(u => u.Id.Equals(userId)).UserName;

                Clients.Users(HubConnections.OnlineUsers()).SendAsync("NewUserOnline", userId, userName);
                HubConnections.AddUserConnection(userId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (HubConnections.HasUserConnection(userId, Context.ConnectionId))
            {
                var userConnections = HubConnections.Users[userId];
                userConnections.Remove(Context.ConnectionId);

                HubConnections.Users.Remove(userId);
                if (userConnections.Any())
                {
                    HubConnections.Users.Add(userId, userConnections);
                }
            }

            if (!string.IsNullOrEmpty(userId))
            {
                string userName = _db.Users.FirstOrDefault(u => u.Id.Equals(userId)).UserName;

                Clients.Users(HubConnections.OnlineUsers()).SendAsync("NewUserDisconnected", userId, userName);
                HubConnections.AddUserConnection(userId, Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }

    }
}
