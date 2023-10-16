using Microsoft.AspNetCore.SignalR;

namespace MySignalR.Hubs
{
    public class HouseHub: Hub
    {
        public static List<string> Subscribtions = new List<string>();

        public async Task JoinHouse(string houseName) 
        {
            if (!Subscribtions.Contains(Context.ConnectionId + ":" + houseName)) 
            {
                Subscribtions.Add(Context.ConnectionId + ":" + houseName);
                string houseList = "";
                foreach (var str in Subscribtions)
                {
                    if (str.Contains(Context.ConnectionId))
                    {
                        houseList += str.Split(':')[1] + " ";
                    }
                }

                await Clients.Caller.SendAsync("SubscriptionStatus", houseList, houseName.ToLower(), true);
                await Clients.Others.SendAsync("MemberAdded", houseName);

                await Groups.AddToGroupAsync(Context.ConnectionId, houseName);
            }
        }

        public async Task LeaveHouse(string houseName)
        {
            if (Subscribtions.Contains(Context.ConnectionId + ":" + houseName))
            {
                Subscribtions.Remove(Context.ConnectionId + ":" + houseName);
                string houseList = "";
                foreach (var str in Subscribtions)
                {
                    if (str.Contains(Context.ConnectionId))
                    {
                        houseList += str.Split(':')[1] + " ";
                    }
                }

                await Clients.Caller.SendAsync("SubscriptionStatus", houseList, houseName.ToLower(), false);
                await Clients.Others.SendAsync("MemberRemoved", houseName);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, houseName);
            }
        }

        public async Task Notify(string house) 
        {
            await Clients.Group(house).SendAsync("TriggerNotification", house);
        }
    }
}
