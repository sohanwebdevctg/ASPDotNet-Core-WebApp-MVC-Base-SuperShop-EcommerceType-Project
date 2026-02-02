using Microsoft.AspNetCore.Mvc;

namespace SuperShop.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //all products
        public IActionResult AllProducts()
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
