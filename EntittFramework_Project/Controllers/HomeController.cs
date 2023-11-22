using Microsoft.AspNetCore.Mvc;

namespace EntittFramework_Project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
