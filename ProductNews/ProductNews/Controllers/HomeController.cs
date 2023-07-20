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

            ViewData["numOfComments"] = context.News.Where(x => x.IsDelete == false && x.Comments.Count >= 1).OrderByDescending(x => x.Comments.Count)
                .Take(Constant.NUMBER_OF_MOST_POPULAR).ToList();

            ViewData["allNews"] = x.ToList();
            ViewData["commentsOfSixNearestNews"] = context.News.Where(x => x.IsDelete == false)
                .OrderByDescending(x => x.CreatedDate)
                .Take(6).ToList();


            //var evaluations = context.Evaluations.Include(x => x.Customer).Where(x => x.NewsId == id).ToList();
            //int totalStars = evaluations.Select(x => x.Rating).Sum().Value;
            //double ratingTotal = totalStars == 0 ? 0 : totalStars * 1.0 / evaluations.Count();

            List<double> oneStars = new List<double>();
            IDictionary<int, double> stars = new Dictionary<int, double>();
            var newssss = context.News.Where(x => x.IsDelete == false).ToList();
            for (int i = 1; i <= 5; i++)
            {
                stars.Add(i, 0);
            }
            foreach (News news in newssss)
            {
                var evals = context.Evaluations.Include(x => x.Customer).Where(x => x.NewsId == news.NewsId).ToList();
                int totalStars = evals.Select(x => x.Rating).Sum().Value;
                double ratingTotal = totalStars == 0 ? 0 : totalStars * 1.0 / evals.Count();
                if (ratingTotal >= 1 && ratingTotal < 2)
                {
                    if (stars.ContainsKey(1))
                    {
                        stars[1] = stars[1] + 1;
                    }
                    else
                    {
                        stars.Add(1, 1);
                    }
                }
                else if (ratingTotal >= 2 && ratingTotal < 3)
                {
                    if (stars.ContainsKey(2))
                    {
                        stars[2] = stars[2] + 1;
                    }
                    else
                    {
                        stars.Add(2, 1);
                    }
                }
                else if (ratingTotal >= 3 && ratingTotal < 4)
                {
                    if (stars.ContainsKey(3))
                    {
                        stars[3] = stars[3] + 1;
                    }
                    else
                    {
                        stars.Add(3, 1);
                    }
                }
                else if (ratingTotal >= 4 && ratingTotal < 5)
                {
                    if (stars.ContainsKey(4))
                    {
                        stars[4] = stars[4] + 1;
                    }
                    else
                    {
                        stars.Add(4, 1);
                    }
                }
                else
                {
                    if (stars.ContainsKey(5))
                    {
                        stars[5] = stars[5] + 1;
                    }
                    else
                    {
                        stars.Add(5, 1);
                    }
                }
            }
            ViewData["stars"] = stars;

            return View();
        }

        public IActionResult GetWhatsNews()
        {
            return View();
        }
    }
}
