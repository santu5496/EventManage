using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    public class TableViewController : Controller
    {
        public IActionResult TableView()
        {
            return View();
        }
    }
}
