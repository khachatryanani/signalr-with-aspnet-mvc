using Microsoft.AspNetCore.SignalR;

namespace MySignalR.Hubs
{
    public class NotificationHub: Hub
    {
        public static int counter { get; set; }
        public static List<string> Messages { get; set; } = new List<string>();

        public async Task SendMessage(string message) 
        {
            if (!string.IsNullOrWhiteSpace(message)) 
            {
                counter++;
                Messages.Add(message);
                await LoadMessages();
            }
        }

        public async Task LoadMessages() 
        {
            await Clients.All.SendAsync("LoadMessages", Messages, counter);
        }
    }
}
