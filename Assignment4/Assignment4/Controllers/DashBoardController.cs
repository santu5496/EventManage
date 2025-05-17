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
            return View();
        }


       
    }
}
