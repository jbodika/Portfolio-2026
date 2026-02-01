using Microsoft.AspNetCore.Mvc;

namespace jbH60Store.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Attributions()
        {
            return View();
        }
    }
}
