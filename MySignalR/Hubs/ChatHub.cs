using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MySignalR.Data;

namespace MySignalR.Hubs
{
    public class ChatHub: Hub
    {
        public readonly ApplicationDbContext _db;
        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SendMessageToAll(string user, string message) 
        {
            await Clients.All.SendAsync("MessageReceived", user, message);
        }

        [Authorize]
        public async Task SendMessageToReceiver(string sender, string receiver, string message)
        {
            string userId = _db.Users.FirstOrDefault(u => u.Email.ToLower().Equals(receiver.ToLower())).Id;

            if (!string.IsNullOrEmpty(userId)) 
            {
                await Clients.Users(userId).SendAsync("MessageReceived", sender, message);
            }
        }
    }
}
