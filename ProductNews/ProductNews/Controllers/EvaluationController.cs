using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using ProductNews.Helpers;
using ProductNews.Models;
using System.Diagnostics;

namespace ProductNews.Controllers
{
    public class EvaluationController : Controller
    {
        private readonly ProductNewsContext context = new ProductNewsContext();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEvaluation(int newsId, int rateNum, string content, string customerName, string email)
        {
            // tìm customer có email trên
            Customer c = new CustomerController().CreateCustomer(customerName, email);
            // kiểm tra xem liệu có khách hàng này từng đánh giá sp này trước đây không
            Evaluation e = context.Evaluations.Where(x => x.NewsId == newsId && x.Customer.Email == email).FirstOrDefault();
            if (e == null)
            {
                e = new Evaluation()
                {
                    NewsId = newsId,
                    Rating = rateNum,
                    Content = content,
                    Status = 1, // mặc định là đánh giá được duyệt
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    //Customer = c
                    CustomerId = c.CustomerId
                };

                context.Evaluations.Add(e);
            }
            else
            {
                // khách hàng đã đánh giá bài viết này
                // chỉ cần cập nhật lại nội dung và rate
                e.Rating = rateNum;
                e.Content = content;
                context.Evaluations.Update(e);
            }

            context.SaveChanges();

            //return context.Evaluations.Where(x => x.EvaluationId == e.EvaluationId).Select(v =>

            //     new PartialEvaluation()
            //     {
            //         EvaluationId = v.EvaluationId,
            //         Rating = v.Rating,
            //         Content = v.Content,
            //         UserName = v.Customer.UserName
            //     }

            // ).ToJson().ToString();

            int count = context.Evaluations.Where(x => x.NewsId == newsId).Count();
            int lastPageNumber = count / Constant.PAGE_SIZE;
            if(count % Constant.PAGE_SIZE > 0)
            {
                lastPageNumber += 1;
            }

            return RedirectToAction("Index", "News", new { id = newsId, page = lastPageNumber });
        }
    }

    class PartialEvaluation
    {
        public int EvaluationId { get; set; }
        public int? Rating { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
    }

}

