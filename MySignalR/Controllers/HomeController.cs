using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MySignalR.Hubs;
using MySignalR.Models;
using System.Diagnostics;

namespace MySignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<VotingHub> _hub;

        public HomeController(ILogger<HomeController> logger, IHubContext<VotingHub> hub)
        {
            _logger = logger;
            _hub = hub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DeathlyHallows(string type) 
        {
            if (SData.Voting.ContainsKey(type)) 
            {
                SData.Voting[type]++;
            }
            _hub.Clients.All.SendAsync("UpdateVotingResults", SData.Voting[SData.Cloak], SData.Voting[SData.Stone], SData.Voting[SData.Wand]);

            return Accepted();
        }
        public IActionResult Notification()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}