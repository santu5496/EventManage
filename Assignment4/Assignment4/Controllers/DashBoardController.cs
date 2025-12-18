using DbOperation.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Assignment4.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IDashBoard _dashBoardService;

        public DashBoardController(IDashBoard dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }
        public IActionResult DashBoard()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("login", "Home");
            }
            return View();
        }


       
    }
}
