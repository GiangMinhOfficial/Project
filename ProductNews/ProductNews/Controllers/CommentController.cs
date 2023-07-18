using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductNews.Helpers;
using ProductNews.Models;
using System.Drawing.Printing;
using X.PagedList;

namespace ProductNews.Controllers
{
    public class CommentController : Controller
    {
        private readonly ProductNewsContext context = new();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateComment(int newsId, string content, string customerName, string email)
        {
            // tìm customer có email trên
            Customer c = new CustomerController().CreateCustomer(customerName, email);
            // kiểm tra xem liệu có khách hàng này từng đánh giá sp này trước đây không
            Comment cmt = new Comment()
            {
                NewsId = newsId,
                Customer = c,
                Content = content,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = 1,
                IsDelete = false
            };
            context.Comments.Add(cmt);

            context.SaveChanges();

            return RedirectToAction("Index", "News", new { id = newsId });
        }

        public IActionResult GetCommentPagedList(int? page, int id)
        {
            page = page ?? 1;
            var pageSize = Constant.PAGE_SIZE;
            var pagedData = context.Comments.Include(x => x.Customer).Where(x => x.NewsId == id).ToPagedList((int)page, pageSize);
            ViewBag.newsId = id;
            return PartialView("_CommentPagedDataPartial", pagedData);
        }
    }
}
