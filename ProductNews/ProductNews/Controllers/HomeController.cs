using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductNews.Models;

namespace ProductNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductNewsContext _context = new();

        //public HomeController(ProductNewsContext context)
        //{
        //    _context = context;
        //}

        public IActionResult Index()
        {
            //var news = _context.News.ToList().DistinctBy(x => x.NewTitle).Take(3).ToList();
            var firstNews = _context.News.Include(x => x.NewsGroup).Take(_context.News.ToList().Count - 2).ToList();
            var lastNews = _context.News.Include(x => x.NewsGroup).Skip(_context.News.ToList().Count - 2).Take(2).ToList();

            ViewBag.firstNews = firstNews;
            ViewBag.lastNews = lastNews;
            return View();
        }
    }
}
