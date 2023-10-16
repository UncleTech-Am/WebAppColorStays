using Microsoft.AspNetCore.Mvc;

namespace WebAppColorStays.Areas.Developer.Controllers
{
    [Area("Developer")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
