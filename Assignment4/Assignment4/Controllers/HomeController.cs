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
            return View();
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

        public IActionResult GetUserByPhoneNumber(string phonenumber,string password)
        {
            var result = _userService.ValidateUser(phonenumber,password);
            if (result == null)
            {
                return Json(new { success = false, message = "No user found." });
            }
            return Json(result);


        }
        public IActionResult AddUser(Users user)
        {
            var result = _userService.AddUser(user);
         
            return Json(result);
        }
    }
}
