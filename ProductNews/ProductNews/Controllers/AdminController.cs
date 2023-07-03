using Microsoft.AspNetCore.Mvc;

namespace ProductNews.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
