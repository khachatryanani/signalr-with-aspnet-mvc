using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MySignalR.Data;
using MySignalR.Hubs;
using MySignalR.Models;
using System.Diagnostics;

namespace MySignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<VotingHub> _votingHub;
        private readonly IHubContext<OrderHub> _orderHub;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, IHubContext<VotingHub> votingHub, IHubContext<OrderHub> orderHub, ApplicationDbContext context)
        {
            _logger = logger;
            _votingHub = votingHub;
            _orderHub = orderHub;
            _context = context;
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
            _votingHub.Clients.All.SendAsync("UpdateVotingResults", SData.Voting[SData.Cloak], SData.Voting[SData.Stone], SData.Voting[SData.Wand]);

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

        [ActionName("Order")]
        public async Task<IActionResult> Order()
        {
            string[] name = { "Bhrugen", "Ben", "Jess", "Laura", "Ron" };
            string[] itemName = { "Food1", "Food2", "Food3", "Food4", "Food5" };

            Random rand = new Random();
            // Generate a random index less than the size of the array.  
            int index = rand.Next(name.Length);

            Order order = new Order()
            {
                Name = name[index],
                ItemName = itemName[index],
                Count = index
            };

            return View(order);
        }

        [ActionName("Order")]
        [HttpPost]
        public async Task<IActionResult> OrderPost(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            _orderHub.Clients.All.SendAsync("newOrder");

            return RedirectToAction(nameof(Order));
        }
        [ActionName("OrderList")]
        public async Task<IActionResult> OrderList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllOrder()
        {
            var productList = _context.Orders.ToList();
            return Json(new { data = productList });
        }
    }
}