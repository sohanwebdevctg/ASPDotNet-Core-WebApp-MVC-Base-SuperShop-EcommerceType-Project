using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Models;
using SuperShop.ViewModels;

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

        
        // show banner, category, product, offer
        public IActionResult Index()
        {
            var viewModel = new HomeVM
            {
                // Only categories that have at least 1 product
                Categories = _context.Categoreis
                             .Where(c => _context.Products.Any(p => p.CategoryId == c.CategoryId))
                             .ToList(),

                // Latest 8 products
                Products = _context.Products.OrderByDescending(p => p.ProductId).Take(12).ToList(),

                // 3 banners (1 main, 2 side)
                Banners = _context.Banners.Take(3).ToList(),

                // 3 offer cards
                Offers = _context.Offers.Take(3).ToList()
            };

            return View(viewModel);
        }

        // partial view
        public IActionResult GetProductsByCategory(int id)
        {
            var productsQuery = _context.Products.AsQueryable();

            if (id > 0)
            {
                
                productsQuery = productsQuery.Where(p => p.CategoryId == id);
            }

            var products = productsQuery
                            .OrderByDescending(p => p.ProductId)
                            .Take(12)
                            .ToList();

            return PartialView("_ProductListPartial", products);
        }

        // product-details
        [HttpGet]
        public IActionResult ProductDetails(int id)
        {

            // validation the product id
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction("AllProduct", "Customer");
            }

            return View(product);
        }

        // userporfile
        public IActionResult UserProfile()
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users
                .Include(r => r.Role)
                .Include(g => g.Gender)
                .Include(c => c.City)
                .Include(c => c.Country)
                .FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            return View(dbUser);
        }

        // update-user
        [HttpGet]
        public IActionResult UpdateUser(int id)
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2 || sessionUserId != id)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // show viewmodel
            var viewModel = new UserEditVM
            {
                UserData = dbUser,
                GenderList = _context.Genders.Select(g => new SelectListItem { Value = g.GenderId.ToString(), Text = g.GenderName }),
                CityList = _context.Cities.Select(g => new SelectListItem { Value = g.CityId.ToString(), Text = g.CityName }),
                CountryList = _context.Countries.Select(g => new SelectListItem { Value = g.CountryId.ToString(), Text = g.CountryName }),
                RoleList = _context.Roles.Select(g => new SelectListItem { Value = g.RoleId.ToString(), Text = g.RoleName })

            };

            return View(viewModel);
        }

        // update-user
        [HttpPost]
        public IActionResult UpdateUser(UserEditVM userModel, IFormFile? imageFile)
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // check user id validation
            if (userModel.UserData.UserId != sessionUserId)
            {
                TempData["Error"] = "Unauthorized Access!";
                return RedirectToAction("UserProfile", "Customer");
            }

            // user data validation
            if (ModelState.IsValid)
            {

                // check user validation
                var existingUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.UserId == userModel.UserData.UserId);

                if (existingUser == null)
                {
                    TempData["Error"] = "User Not Found";
                    return RedirectToAction("UserProfile", "Customer");
                }

                // create folder path
                string folder = Path.Combine(_env.WebRootPath, "images", "user_img");

                // delete previous image
                if (imageFile != null)
                {
                    
                    if (!string.IsNullOrEmpty(existingUser.UserImage) && existingUser.UserImage != "no-user.png")
                    {
                        string oldFilePath = Path.Combine(folder, existingUser.UserImage);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // save new image
                    string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fs);
                    }
                    userModel.UserData.UserImage = fileName;
                }
                else
                {
                    // set previous image
                    userModel.UserData.UserImage = existingUser.UserImage;
                }

                
                userModel.UserData.UserStatus = existingUser.UserStatus;
                userModel.UserData.RoleId = existingUser.RoleId;

                // update database
                _context.Users.Update(userModel.UserData);
                _context.SaveChanges();

                return RedirectToAction("UserProfile", "Customer");

            }

            // return viewmodel
            userModel.GenderList = _context.Genders.Select(g => new SelectListItem { Value = g.GenderId.ToString(), Text = g.GenderName });
            userModel.CityList = _context.Cities.Select(c => new SelectListItem { Value = c.CityId.ToString(), Text = c.CityName });
            userModel.CountryList = _context.Countries.Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.CountryName });
            userModel.RoleList = _context.Roles.Select(r => new SelectListItem { Value = r.RoleId.ToString(), Text = r.RoleName });


            return View(userModel);
        }

        //allproducts
        [HttpGet]
        public IActionResult AllProducts(string searchName, decimal? searchPrice)
        {
            // all product data load
            var productsQuery = _context.Products.AsQueryable();

            // search product name
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(searchName));
                ViewBag.CurrentName = searchName;
            }

            // search product price
            if (searchPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductPrice <= searchPrice.Value);
                ViewBag.CurrentPrice = searchPrice;
            }

            var results = productsQuery.OrderByDescending(p => p.ProductId).ToList();

            // sending the warning message
            if (!results.Any())
            {
                ViewBag.Message = "Product not found!";
            }

            // return the previous product
            return View(results);
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

        // buy-now
        [HttpPost]
        public IActionResult BuyNow(int id)
        {
            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // order validation
            var product = _context.Products.Find(id);
            if (product == null || product.ProductLimit <= 0)
            {
                TempData["Error"] = "Sorry, this product is out of stock!";
                return RedirectToAction("ProductDetails", "Customer", new { id = id });
            }

            // see user order panding
            var existingOrder = _context.Orders.FirstOrDefault(o => o.UserId == sessionUserId);

            if (existingOrder == null)
            {
                // Create new order master
                var newOrder = new Order
                {
                    UserId = sessionUserId,
                    UserName = dbUser.UserName ?? "Unknown User",
                    UserEmail = dbUser.UserEmail,
                    UserImage = dbUser.UserImage ?? "no-user.png",
                    OrderDate = DateTime.Now,
                    TotalAmount = product.ProductPrice
                };
                _context.Orders.Add(newOrder);
                _context.SaveChanges();

                // product details save
                var detail = new OrderDetails
                {
                    OrderId = newOrder.OrderId,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                    Quantity = 1
                };
                _context.OrderDetails.Add(detail);
            }
            else
            {
               
                var existingDetail = _context.OrderDetails
                    .FirstOrDefault(d => d.OrderId == existingOrder.OrderId && d.ProductId == id);

                if (existingDetail != null)
                {
                    existingDetail.Quantity += 1;
                }
                else
                {
                    var detail = new OrderDetails
                    {
                        OrderId = existingOrder.OrderId,
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductImage = product.ProductImage,
                        ProductPrice = product.ProductPrice,
                        Quantity = 1
                    };
                    _context.OrderDetails.Add(detail);
                }

                
                existingOrder.TotalAmount += product.ProductPrice;
            }

            
            product.ProductLimit -= 1;

            _context.SaveChanges();

            TempData["Success"] = "Product Added SuccessFully!";
            return RedirectToAction("ProductDetails", "Customer", new { id = id });

        }

        // order-table
        public IActionResult Order()
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            var userOrders = _context.Orders.Include(o => o.OrderDetails).Where(o => o.UserId == sessionUserId).OrderByDescending(o => o.OrderDate).ToList();

            if(userOrders.Count == 0)
            {
                ViewBag.message = "No Data Here!";
            }

            return View(userOrders);
        }


        // delete-order-item
        [HttpPost]
        public IActionResult DeleteOrderItem(int id)
        {
            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            var orderDetail = _context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id);

            if (orderDetail != null)
            {
                // update product logic
                var product = _context.Products.FirstOrDefault(p => p.ProductId == orderDetail.ProductId);
                if (product != null)
                {
                    product.ProductLimit += orderDetail.Quantity;
                    _context.Products.Update(product);
                }

                // update order
                var mainOrder = _context.Orders.FirstOrDefault(o => o.OrderId == orderDetail.OrderId);
                if (mainOrder != null)
                {
                    mainOrder.TotalAmount -= (orderDetail.ProductPrice * orderDetail.Quantity);
                    _context.Orders.Update(mainOrder);
                }

                // remove order
                _context.OrderDetails.Remove(orderDetail);
                _context.SaveChanges();

                // success message
                TempData["Success"] = "Product Delete Successfully";
            }

            return RedirectToAction("Order", "Customer");


        }

        // payment
        [HttpGet]
        public IActionResult Payment()
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // get all pending data
            var userOrders = _context.Orders.Where(o => o.UserId == sessionUserId).ToList();

            if (!userOrders.Any())
            {
                TempData["Error"] = "No orders found to pay!";
                return RedirectToAction("Order", "Customer");
            }

            // create payment object to show payment form
            var paymentData = new Order
            {
                UserId = dbUser.UserId,
                UserName = dbUser.UserName,
                UserEmail = dbUser.UserEmail,
                TotalAmount = userOrders.Sum(o => o.TotalAmount)
            };

            return View(paymentData);
        }

        // process-payment
        [HttpPost]
        public IActionResult ProcessPayment(Payment payment)
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // payment
            if (payment != null)
            {
                _context.Payments.Add(payment);
                _context.SaveChanges();

                // delete all data form order table
                var userOrders = _context.Orders.Where(o => o.UserId == payment.UserId).ToList();

                foreach (var order in userOrders)
                {
                    var details = _context.OrderDetails.Where(d => d.OrderId == order.OrderId).ToList();
                    _context.OrderDetails.RemoveRange(details);
                }

                // delete main order
                _context.Orders.RemoveRange(userOrders);
                _context.SaveChanges();

                TempData["Success"] = "Payment Completed Successfully!";
                return RedirectToAction("Order", "Customer");
            }

            TempData["Error"] = "Payment Failed! Please try again.";
            return RedirectToAction("Payment", "Customer");
        }










    }
}
