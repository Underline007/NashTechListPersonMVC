using Microsoft.AspNetCore.Mvc;

namespace NashTechListPersonMVC.WebApp.Areas.NashTech.Controllers
{
    [Area("NashTech")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
