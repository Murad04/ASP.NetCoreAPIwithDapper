using Microsoft.AspNetCore.Mvc;

namespace ASP.NetCoreAPIwithDapper.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
