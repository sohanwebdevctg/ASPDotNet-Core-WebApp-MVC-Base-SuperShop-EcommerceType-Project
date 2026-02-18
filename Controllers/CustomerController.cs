using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;
using SuperShop.Models;

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

        // contact-form
        public IActionResult Contact()
        {
            // user id check from session
            var userId = HttpContext.Session.GetInt32("UserId");

            // validation the user id
            if (userId != null)
            {
                var user = _context.Users.FirstOrDefault(x => x.UserId == userId);

                // take userName & userEmail
                if(user != null)
                {
                    ViewBag.userName = user.UserName;
                    ViewBag.userEmail = user.UserEmail;
                }
            }

            return View();
        }

        // contact-form-submit
        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            // model validation check
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                TempData["Success"] = "Your Data Submit Successfully!";
                return RedirectToAction("Contact", "Customer");
            }

            // error message send
            TempData["Error"] = "Please Fill Up The Full Data";

            // return the user data
            ViewBag.userName = contact.UserName;
            ViewBag.userEmail = contact.UserEmail;

            return View(contact);
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
