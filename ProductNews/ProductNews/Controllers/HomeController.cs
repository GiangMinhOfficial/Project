using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ProductNews.Models;
using System.Diagnostics;

namespace ProductNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductNewsContext _context = new();

        public IActionResult Index()
        {
            var x = _context.News.Where(x => x.IsDelete == false).Include(x => x.NewsGroup)
                .Include(x => x.CreatedByNavigation);
            int count = _context.News.Where(x => x.IsDelete == false).ToList().Count;
            //var news = _context.News.ToList().DistinctBy(x => x.NewTitle).Take(3).ToList();
            var firstNewsList = x.Take(count - 2).ToList();
            var lastNewsList = x.Skip(count - 2).Take(2).ToList();

            ViewBag.firstNews = firstNewsList;
            ViewBag.lastNews = lastNewsList;
            return View();
        }
    }
}
