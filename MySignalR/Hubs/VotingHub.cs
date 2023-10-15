using Microsoft.AspNetCore.SignalR;

namespace MySignalR.Hubs
{
    public class VotingHub: Hub
    {
        public Dictionary<string, int> GetVotingResults() 
        {
            return SData.Voting;
        }
    }
}
