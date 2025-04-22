using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    public class EventBookingController : Controller
    {
        public IActionResult EventBooking()
        {
            return View();
        }
    }
}
