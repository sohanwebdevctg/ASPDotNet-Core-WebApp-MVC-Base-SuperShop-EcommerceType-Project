using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;

namespace SuperShop.Controllers
{
    public class CustomerController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CustomerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index()
        {
            return View();
        }

        // userporfile
        public IActionResult UserProfile()
        {
            return View();
        }

        //allproducts
        public IActionResult AllProducts()
        {
            return View();
        }

        //productdetails
        public IActionResult ProductDetails()
        {
            return View();
        }

        //about
        public IActionResult About()
        {
            return View();
        }

        // contact
        public IActionResult Contact()
        {
            return View();
        }

        // blog
        public IActionResult Blog()
        {
            return View();
        }

        // order-table
        public IActionResult Order()
        {
           return View();
        }
    }
}
