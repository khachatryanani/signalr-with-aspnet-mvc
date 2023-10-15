using Microsoft.AspNetCore.SignalR;

namespace MySignalR.Hubs
{
    public class UserHub: Hub
    {
        private static int TotalViews = 0;
        private static int TotalUsers = 0;

        public override Task OnConnectedAsync()
        {
            TotalUsers++;
            Clients.All.SendAsync("UpdateTotalUsers", TotalUsers).GetAwaiter().GetResult();

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            TotalUsers--;
            Clients.All.SendAsync("UpdateTotalUsers", TotalUsers).GetAwaiter().GetResult();

            return base.OnDisconnectedAsync(exception);
        }
        public async Task NewWindowLoaded() 
        {
            TotalViews++;

            // send update to all clients
            await Clients.All.SendAsync("UpdateTotalViews", TotalViews);
        }
    }
}
