using Microsoft.AspNetCore.Mvc;

namespace HalloDocMVC.Controllers
{
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
