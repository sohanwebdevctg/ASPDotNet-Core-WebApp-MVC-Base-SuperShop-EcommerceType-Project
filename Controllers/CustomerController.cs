using Microsoft.AspNetCore.Mvc;

namespace SuperShop.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //allproducts
        public IActionResult AllProducts()
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
    }
}
