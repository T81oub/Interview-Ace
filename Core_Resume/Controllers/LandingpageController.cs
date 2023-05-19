using Microsoft.AspNetCore.Mvc;

namespace Core_Resume.Controllers
{
    public class LandingpageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Features()
        {
            return View();
        }
        public IActionResult Contactus()
        {
            return View();
        }

    }
}
