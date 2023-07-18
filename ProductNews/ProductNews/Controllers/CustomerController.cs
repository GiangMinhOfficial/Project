using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using ProductNews.Models;

namespace ProductNews.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ProductNewsContext context = new();

        public CustomerController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public Customer CreateCustomer(string customerName, string email)
        {
            Customer c = context.Customers.FirstOrDefault(x => x.Email.Equals(email));

            if (c == null)
            {
                string[] names = customerName.Split(' ');

                c = NewCustomer(new Customer()
                {
                    LastName = names[0],
                    FirstName = names.Length >= 2 ? string.Join("", names[1..]) : names[0],
                    Email = email,
                    UserName = customerName,
                    IsDelete = false,
                    CreatedDate = DateTime.Now
                });

            }

            return c;
        }

        public void Login(Customer c)
        {
            Customer oldCustomer = context.Customers.FirstOrDefault(x => x.Email.Equals(c.Email));

            if (oldCustomer == null)
            {
                oldCustomer = NewCustomer(c);
            }

            HttpContext.Session.SetString("customer", c.ToJson());
        }

        public IActionResult Logout(string url)
        {
            HttpContext.Session.Remove("customer");
            return Redirect(url);
        }

        private Customer? NewCustomer(Customer c)
        {
            Customer oldCustomer = context.Customers.FirstOrDefault(x => x.Email.Equals(c.Email)) ?? new Customer();

            oldCustomer.LastName = c.LastName;
            oldCustomer.FirstName = c.FirstName;
            oldCustomer.Email = c.Email;
            oldCustomer.UserName = c.UserName;
            oldCustomer.IsDelete = false;
            oldCustomer.CreatedDate = DateTime.Now;
            oldCustomer.Avatar = c.Avatar;

            return oldCustomer;
        }
    }
}
