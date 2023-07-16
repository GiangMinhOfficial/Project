﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using ProductNews.Helpers;
using ProductNews.Models;
using X.PagedList;

namespace ProductNews.Controllers
{
    public class NewsController : Controller
    {
        private readonly ProductNewsContext context = new();
        private readonly int PAGE_SIZE = 5;

        public IActionResult Index(int id, int? page)
        {

            if (page == null)
            {
                page = 1;
            }
            int pageSize = PAGE_SIZE;

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


            if (comments.Count > 0)
            {
                ViewBag.comments = comments;
            }

            ViewData["content"] = news.NewsContent;
            ViewData["pagingEvaluations"] = evaluations.ToPagedList((int)page, pageSize);

            return View(news);
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

                ViewBag.evaluations = evaluations;
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
            return View(context.News.Include(x => x.NewsGroup).ToList());
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
            oldNews.ModifiedBy = 1;
            oldNews.View = news.View;

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
            var pageSize = PAGE_SIZE;
            var pagedData = context.Evaluations.Include(x => x.Customer).Where(x => x.NewsId == id).ToPagedList((int)page, pageSize);
            ViewBag.newsId = id;
            return PartialView("_EvaluationPagedDataPartial", pagedData);
        }
    }
}
