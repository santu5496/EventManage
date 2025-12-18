using System.Diagnostics;
using Assignment4.Models;
using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
      

        public IActionResult login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("DashBoard", "DashBoard");
            }
            return View();
        }
        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult GetUserByUserName(string username,string password)
        {
            if (username == "Admin" && password == "Admin123")
            {
                var adminUser = new Users
                {
                    userId = 1,
                    fullName = "Administrator",
                    email = "admin@example.com",
                    phoneNumber = "1234567890",
                    userRole = "Admin",
                    username = "Admin"
                };
                HttpContext.Session.SetInt32("UserId", adminUser.userId);
                HttpContext.Session.SetString("Username", adminUser.username);
                HttpContext.Session.SetString("UserRole", adminUser.userRole);
                return Json(new { success = true, message = "User found. Login successful.", user = adminUser });
            }
            
            try
            {
                var result = _userService.ValidateUser(username, password);
                if (result == null)
                {
                    return Json(new { success = false, message = "No user found." });
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", result.userId);
                    HttpContext.Session.SetString("Username", result.username);
                    HttpContext.Session.SetString("UserRole", result.userRole);
                    return Json(new { success = true, message = "User found. Login successful.", user = result });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Database connection error. Use Admin/Admin123 to test." });
            }
        


        }
        public IActionResult AddUser(Users user)
        {
            var result = _userService.AddUser(user);
         
            return Json(result);
        }
    }
}
