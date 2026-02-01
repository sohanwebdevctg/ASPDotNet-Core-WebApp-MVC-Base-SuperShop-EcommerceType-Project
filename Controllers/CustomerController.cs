using Microsoft.AspNetCore.Mvc;

namespace SuperShop.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
