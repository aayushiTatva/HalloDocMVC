using HalloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDocMVC.Controllers
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

        public IActionResult resetPassword2()
        {
            return View();
        }
        public IActionResult loginPage()
        {
            return View();
        }

        public IActionResult SubmitRequestPage()
        {
            return View("../Request/SubmitRequestPage");
        }

        public IActionResult RequestDemo()
        {
            return View("../Request/RequestDemo");
        }
        public IActionResult ReviewAgreement()
        {
            return View("../Home/ReviewAgreement");
        }
        public IActionResult AdminDashboard()
        {
            return View("../Admin/Dashboard/Index");
        }
        public IActionResult ViewCase()
        {
            return View("../Admin/Dashboard/ViewCase");
        }

        public IActionResult ViewNotes()
        {
            return View("../Admin/Dashboard/ViewNotes");
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