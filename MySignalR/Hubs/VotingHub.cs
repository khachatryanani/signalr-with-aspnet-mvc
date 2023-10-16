using Microsoft.AspNetCore.SignalR;

namespace MySignalR.Hubs
{
    public class VotingHub: Hub
    {
        public async Task<Dictionary<string, int>> GetVotingResults() 
        {
            return SData.Voting;
        }
    }
}
