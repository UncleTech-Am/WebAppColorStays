using LibAuthService.ModelView;
using LibCommon.DataTransfer;
using LibCommon.Service;
using Microsoft.AspNetCore.Mvc;

namespace WebAppColorStays.Areas.ColorStays.Controllers
{
    [Area("ColorStays")]
    [SessionCheck]

    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    
    }
}
