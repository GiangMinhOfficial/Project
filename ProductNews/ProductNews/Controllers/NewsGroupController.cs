using Microsoft.AspNetCore.Mvc;
using ProductNews.Models;

namespace ProductNews.Controllers
{
    public class NewsGroupController : Controller
    {
        private readonly ProductNewsContext context = new();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetGroupOfNewsPartialView()
        {
            var newsGroups = context.NewsGroups.Take(5).ToList();

            return PartialView("", newsGroups);
        }
    }
}
