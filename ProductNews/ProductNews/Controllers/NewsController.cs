using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using ProductNews.Models;

namespace ProductNews.Controllers
{
    public class NewsController : Controller
    {
        private readonly ProductNewsContext context = new();

        public IActionResult Index(int id)
        {
            // get all comments having newsid equal to id
            var comments = context.Comments.Include(x => x.Customer).Where(x => x.NewsId == id).ToList();
            // get all evaluations having evaluationId == commentId that check if newsId in comment table == id
            var evaluations = context.Evaluations.Include(x => x.Customer).Where(x => x.NewsId == id).ToList();
            // get news having newsId == given id
            var news = context.News.Include(x => x.NewsGroup).Where(x => x.NewsId == id).FirstOrDefault();

            int? fiveStarPercent = null;
            int? fourStarPercent = null;
            int? threeStarPercent = null;
            int? twoStarPercent = null;
            int? oneStarPercent = null;
            int totalStars = 0;
            double ratingTotal = 0;


            if (evaluations.Count > 0)
            {
                totalStars = evaluations.Select(x => x.Rating).Sum().Value;
                ratingTotal = totalStars == 0 ? 0 : totalStars * 1.0 / evaluations.Count();

                fiveStarPercent = evaluations.Where(x => x.Rating == 5).Count() * 100 / evaluations.Count();
                fourStarPercent = evaluations.Where(x => x.Rating == 4).Count() * 100 / evaluations.Count();
                threeStarPercent = evaluations.Where(x => x.Rating == 3).Count() * 100 / evaluations.Count();
                twoStarPercent = evaluations.Where(x => x.Rating == 2).Count() * 100 / evaluations.Count();
                oneStarPercent = evaluations.Where(x => x.Rating == 1).Count() * 100 / evaluations.Count();

                ViewBag.evaluations = evaluations;
            }

            ViewData["totalStars"] = totalStars;
            ViewData["ratingTotal"] = ratingTotal;
            ViewData["fiveStarPercent"] = fiveStarPercent ?? 0;
            ViewData["fourStarPercent"] = fourStarPercent ?? 0;
            ViewData["threeStarPercent"] = threeStarPercent ?? 0;
            ViewData["twoStarPercent"] = twoStarPercent ?? 0;
            ViewData["oneStarPercent"] = oneStarPercent ?? 0;

            if (comments.Count > 0)
            {
                ViewBag.comments = comments;
            }

            ViewData["content"] = news.NewsContent;

            return View(news);
        }

        public IActionResult NewsManagement()
        {
            return View(context.News.Include(x => x.NewsGroup).ToList());
        }

        public IActionResult CreateNews()
        {
            var newsGroups = context.NewsGroups.ToList();
            ViewBag.newsGroups = newsGroups;
            return View();
        }

        [HttpPost]
        public ActionResult CreateNews(News news, IFormFile NewsPreviewImage)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();

            string startupPath2 = Environment.CurrentDirectory;

            if (NewsPreviewImage.Length > 0)
            {
                string filePath = Path.Combine("./wwwroot/images/", NewsPreviewImage.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    NewsPreviewImage.CopyTo(fileStream);
                }
            }

            if (ModelState.IsValid)
            {
                news.CreatedDate = DateTime.Now;
                news.ModifiedDate = DateTime.Now;
                news.CreatedBy = 1;
                news.ModifiedBy = 1;
                news.NewsPreviewImage = NewsPreviewImage.FileName;
                news.IsDelete = false;
                news.View = 0;

                context.News.Add(news);
            }

            context.SaveChanges();

            return RedirectToAction("NewsManagement");
        }
    }
}
