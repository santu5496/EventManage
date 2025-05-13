using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    public class EventCrudController : Controller
    {
        private readonly IEventBookingSerive _eventBookingSerive;

        public EventCrudController(IEventBookingSerive eventBookingSerive)
        {
            _eventBookingSerive = eventBookingSerive;
        }

        public IActionResult EventBooking()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddOrUpdateEventBooking(Bookings eventBooking)
        {
            if (eventBooking.eventId != 0)
            {
                var result = _eventBookingSerive.UpdateBooking(eventBooking);
                if (!result)
                {
                    return Json(new { success = false, message = "Failed to update event booking." });
                }
                return Json(new { success = true, message = "Event booking updated successfully." });
            }
            else
            {
                var result = _eventBookingSerive.AddEventBooking(eventBooking);
                if (!result)
                {
                    return Json(new { success = false, message = "Failed to add event booking." });
                }
                return Json(new { success = true, message = "Event booking added successfully." });
            }
        }

        [HttpPost]
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
            return Json(result ?? new List<Bookings>());
        }

        [HttpGet]
        public IActionResult GetBookingByID(int id)
        {
            var result = _eventBookingSerive.GetBookingByID(id);
            return Json(result ?? new List<Bookings>());
        }
    }
}
