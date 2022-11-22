using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO.Ports;
using WebClient.Db;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _db;
        private readonly Port _port;
        private Terminal _terminal;

        public HomeController(ILogger<HomeController> logger, DataContext db,
            Port port, Terminal terminal)
        {
            _logger = logger;
            _port = port;
            _db = db;
            _terminal = terminal;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string text, string port)
        {
            if (text != null)
            {
                _port.PortName = port ?? "empty";

                Model model = new()
                {
                    InData = text,
                    isSended = false,
                    InDateTime = DateTime.UtcNow
                };

                lock (_port.Sync)
                {
                    _db.Models.Add(model);
                    _db.SaveChanges();
                    _db.Entry(model).State = EntityState.Detached;
                }
                ViewData["result"] = "sended";
                string input = model.InDateTime + "> " + model.InData;
                _terminal.log.Add(input);
            }
            ViewData["log"] = _terminal.log;
            return View();
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