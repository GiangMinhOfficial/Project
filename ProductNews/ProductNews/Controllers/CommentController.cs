using Microsoft.AspNetCore.Mvc;
using ProductNews.Models;

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
    }
}
