using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HistorySiteIdentity.Models;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace HistorySiteIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MainIndex()
        {
            return View();
        }
        public IActionResult OneColumn()
        {
            return View();
        }
        public IActionResult TwoColumn1()
        {
            return View();
        }
        public IActionResult TwoColumn2()
        {
            return View();
        }
        public IActionResult ThreeColumn()
        {
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
