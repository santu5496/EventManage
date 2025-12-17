using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    public class EventBookingController : Controller
    {
        private readonly IEventBookingSerive _eventBookingSerive;
        public EventBookingController(IEventBookingSerive eventBookingSerive)
        {
            _eventBookingSerive = eventBookingSerive;
        }
        public IActionResult EventBooking()
        {
            return View();
        }
        public IActionResult TableView()
        {
            return View("TableView"); // Renders TableView.cshtml
        }

        [HttpPost]
        public IActionResult AddOrUpdateEventBooking(Bookings eventBooking)
        {
            if (eventBooking.bookingId != 0)
            {
                var result = _eventBookingSerive.AddBooking(eventBooking);
                if (!result)
                {
                    var errorMsg = _eventBookingSerive.GetLastError();
                    return Json(new { success = false, message = string.IsNullOrEmpty(errorMsg) ? "Failed to update event booking." : errorMsg });
                }
                return Json(new { success = true, message = "Event booking updated successfully." });
            }
            else
            {
                var result = _eventBookingSerive.AddBooking(eventBooking);
                if (!result)
                {
                    var errorMsg = _eventBookingSerive.GetLastError();
                    return Json(new { success = false, message = string.IsNullOrEmpty(errorMsg) ? "Failed to add event booking." : errorMsg });
                }
                return Json(new { success = true, message = "Event booking added successfully." });
            }
        }
        public IActionResult Delete(int id)
        {
            var result = _eventBookingSerive.DeleteBooking(id);
            if (!result)
            {
                return Json(new { success = false, message = "Failed to delete event booking." });
            }
            return Json(new { success = true, message = "Event booking deleted successfully." });
        }
        [HttpGet]
        public IActionResult GetAllEventBookings()
        {
            var result = _eventBookingSerive.GetAllBookings();
            if (result == null || result.Count == 0)
            {
                return Json(result);
            }
            return Json(result);


        }
        public IActionResult GetBookingByID(int id)
        {
            var result = _eventBookingSerive.GetBookingByID(id);
            if(result==null || result.Count==0)
            {
                return Json(result);
            }
            return Json(result);

        }
       
    }
}
