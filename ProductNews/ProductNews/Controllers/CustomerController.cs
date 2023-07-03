using Microsoft.AspNetCore.Mvc;
using ProductNews.Models;

namespace ProductNews.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ProductNewsContext context;

        public CustomerController(ProductNewsContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public Customer CreateCustomer(string customerName, string email)
        {
            Customer c = context.Customers.FirstOrDefault(x => x.Email.Equals(email));

            // tồn tại bug là nếu c != null mà customerName khác với trong db thì cũng không được đổi
            // nghĩa là customerName không có tác dụng
            if (c == null)
            {
                c = new Customer()
                {
                    LastName = customerName,
                    FirstName = customerName,
                    Email = email,
                    UserName = customerName,
                    IsDelete = false,
                    CreatedDate = DateTime.Now
                };
                context.Customers.Add(c);
            }

            context.SaveChanges();

            return c;
        }
    }
}
