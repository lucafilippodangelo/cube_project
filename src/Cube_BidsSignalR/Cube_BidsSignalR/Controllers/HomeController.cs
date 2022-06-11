using Cube_BidsSignalR.CustomSignalR;
using Cube_BidsSignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cube_BidsSignalR.Controllers
{
    public class HomeController : Controller
    {
        private IHubContext<AnHub> _hub;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHubContext<AnHub> hub)
        {
            _hub = hub;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //LD this is to test that the message is sent and visualized after page load and signalr client is connected
            var t = Task.Run(() => ShowThreadInfo());
            
            return View();
            
        }

        private void ShowThreadInfo()
        {
            Thread.Sleep(2000);
            _hub.Clients.All.SendAsync("ReceiveMessage", "user", "message");
        }


        public IActionResult Privacy()
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
