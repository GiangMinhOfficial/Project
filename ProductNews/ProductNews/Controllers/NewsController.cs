using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using ProductNews.Helpers;
using ProductNews.Models;
using X.PagedList;

namespace ProductNews.Controllers
{
    public class NewsController : Controller
    {
        private readonly ProductNewsContext context = new();

        public IActionResult Index(int id, int? page)
        {

            if (page == null)
            {
                page = 1;
            }
            int pageSize = Constant.PAGE_SIZE;

            // get all comments having newsid equal to id
            var comments = context.Comments.Include(x => x.Customer).Where(x => x.NewsId == id).ToList();
            // get all evaluations having evaluationId == commentId that check if newsId in comment table == id
            var evaluations = context.Evaluations.Include(x => x.Customer).Where(x => x.NewsId == id).ToList();
            // get news having newsId == given id
            var news = context.News.Include(x => x.NewsGroup).Where(x => x.NewsId == id).FirstOrDefault();
            news.View += 1;
            context.News.Update(news);
            context.SaveChanges();

            evaluate(evaluations);

            ViewData["content"] = news.NewsContent;
            ViewData["pagingEvaluations"] = evaluations.ToPagedList((int)page, pageSize);
            ViewData["pagingComments"] = comments.ToPagedList((int)page, pageSize);
            ViewData["recentNews"] = context.News
                .Where(x => x.NewsId != news.NewsId && x.IsDelete == false)
                .OrderByDescending(x => x.CreatedDate)
                .Take(Constant.NUMBER_OF_RECENT_NEWS).ToList();
            ViewData["newsGroups"] = context.NewsGroups.Take(Constant.NUMBER_OF_GROUP_NEWS).ToList();

            return View(news);
        }

        public IActionResult List(string search)
        {
            search = search ?? string.Empty;
            search = search.Trim();
            ViewData["search"] = search;

            ViewData["recentNews"] = context.News
                .Where(x => x.IsDelete == false)
                .OrderByDescending(x => x.CreatedDate)
                .Take(Constant.NUMBER_OF_RECENT_NEWS).ToList();

            ViewData["newsGroups"] = context.NewsGroups.Take(Constant.NUMBER_OF_GROUP_NEWS).ToList();

            var newsList = context.News
                .Include(x => x.Comments)
                .Include(x => x.Evaluations)
                .Where(x => x.NewsTitle!.ToLower().Contains(search.ToLower()))
                .ToList();
            return View(newsList);
        }

        private void evaluate(List<Evaluation> evaluations)
        {

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

                //ViewBag.evaluations = evaluations;
            }

            ViewData["totalStars"] = totalStars;
            ViewData["ratingTotal"] = ratingTotal;
            ViewData["fiveStarPercent"] = fiveStarPercent ?? 0;
            ViewData["fourStarPercent"] = fourStarPercent ?? 0;
            ViewData["threeStarPercent"] = threeStarPercent ?? 0;
            ViewData["twoStarPercent"] = twoStarPercent ?? 0;
            ViewData["oneStarPercent"] = oneStarPercent ?? 0;
        }

        public IActionResult NewsManagement()
        {
            if (!RoleCheck())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(context.News.Include(x => x.NewsGroup).OrderBy(x => x.NewsGroupId).ToList());
        }

        public IActionResult CreateNews()
        {
            if (!RoleCheck())
            {
                return RedirectToAction("Index", "Home");
            }
            var newsGroups = context.NewsGroups.ToList();
            ViewBag.newsGroups = newsGroups;
            return View();
        }

        [HttpPost]
        public ActionResult CreateNews(News news, IFormFile NewsPreviewImage)
        {
            if (!RoleCheck())
            {
                return RedirectToAction("Index", "Home");
            }
            if (NewsPreviewImage.Length > 0)
            {
                news.NewsPreviewImage = Utility.SaveImage(NewsPreviewImage);
            }

            if (ModelState.IsValid)
            {
                news.CreatedDate = DateTime.Now;
                news.ModifiedDate = DateTime.Now;
                news.CreatedBy = 1;
                news.ModifiedBy = 1;
                news.IsDelete = false;
                news.View = 0;

                context.News.Add(news);
            }

            context.SaveChanges();

            return RedirectToAction("NewsManagement");
        }

        public IActionResult EditNews(int newsId)
        {
            if (!RoleCheck())
            {
                return RedirectToAction("Index", "Home");
            }
            var newsGroups = context.NewsGroups.ToList();
            ViewBag.newsGroups = newsGroups;

            var news = context.News.FirstOrDefault(x => x.NewsId == newsId);
            return View(news);
        }

        [HttpPost]
        public IActionResult EditNews(News news, IFormFile? NewsPreviewImage)
        {
            if (!RoleCheck())
            {
                return RedirectToAction("Index", "Home");
            }
            var oldNews = context.News.FirstOrDefault(x => x.NewsId == news.NewsId);

            if (NewsPreviewImage != null && NewsPreviewImage.Length > 0)
            {
                oldNews.NewsPreviewImage = Utility.SaveImage(NewsPreviewImage);
            }

            oldNews.NewsTitle = news.NewsTitle;
            oldNews.NewsGroupId = news.NewsGroupId;
            oldNews.NewsContent = news.NewsContent;
            oldNews.ModifiedDate = DateTime.Now;
            oldNews.ModifiedBy = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("admin")).AccountId;

            context.SaveChanges();

            return RedirectToAction("NewsManagement");
        }

        public IActionResult DeleteNews(int newsId)
        {
            if (!RoleCheck())
            {
                return RedirectToAction("Index", "Home");
            }
            var news = context.News.FirstOrDefault(x => x.NewsId == newsId);
            if (news != null)
            {
                news.IsDelete = true;
                context.News.Update(news);
                context.SaveChanges();
            }
            return RedirectToAction("NewsManagement");
        }

        public IActionResult RestoreNews(int newsId)
        {
            if (!RoleCheck())
            {
                return RedirectToAction("Index", "Home");
            }
            var news = context.News.FirstOrDefault(x => x.NewsId == newsId);
            if (news != null)
            {
                news.IsDelete = false;
                context.News.Update(news);
                context.SaveChanges();
            }
            return RedirectToAction("NewsManagement");
        }

        private bool RoleCheck()
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return false;
            }

            return true;
        }

        public IActionResult GetEvaluationPagedDate(int? page, int id)
        {
            page = page ?? 1;
            var pageSize = Constant.PAGE_SIZE;
            var pagedData = context.Evaluations.Include(x => x.Customer).Where(x => x.NewsId == id).ToPagedList((int)page, pageSize);
            ViewBag.newsId = id;
            return PartialView("_EvaluationPagedDataPartial", pagedData);
        }

        //public IActionResult GetRecentNewsPartialView(int id, int? currentNewsId)
        //{
        //    var recentNews = context.News.OrderByDescending(x => x.CreatedDate).Where(x => x.NewsId != currentNewsId).Take(4).ToList();
        //    return PartialView("_RecentNewsPartial", recentNews);
        //}
    }
}
