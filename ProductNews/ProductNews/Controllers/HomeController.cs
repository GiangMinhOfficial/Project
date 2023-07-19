using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ProductNews.Helpers;
using ProductNews.Models;
using System.Diagnostics;

namespace ProductNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductNewsContext context = new();

        public IActionResult Index()
        {
            var x = context.News.Where(x => x.IsDelete == false).Include(x => x.NewsGroup)
                .Include(x => x.CreatedByNavigation);
            int count = x.Count();
            //var news = context.News.ToList().DistinctBy(x => x.NewTitle).Take(3).ToList();
            var firstNewsList = x.Take(count - 2).ToList();
            var lastNewsList = x.Skip(count - 2).Take(2).ToList();

            ViewData["newsGroups"] = context.NewsGroups.Take(Constant.NUMBER_OF_GROUP_NEWS).ToList();

            var latestNews = context.News.Where(x => x.IsDelete == false).OrderByDescending(x => x.CreatedDate);
            ViewData["whatsNews"] = latestNews.ToList();

            ViewData["firstNews"] = firstNewsList;
            ViewData["lastNews"] = lastNewsList;

            ViewData["mostPopular"] = context.News.Where(x => x.IsDelete == false).OrderByDescending(x => x.View).
                Take(Constant.NUMBER_OF_MOST_POPULAR).ToList();

            ViewData["allNews"] = x.ToList();
            return View();
        }

        public IActionResult GetWhatsNews()
        {
            return View();
        }
    }
}
