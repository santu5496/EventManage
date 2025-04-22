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
        [HttpPost]
        public IActionResult AddOrUpdateEventBooking(Events eventBooking)
        {
            if (eventBooking.eventId != 0)
            {
                var result = _eventBookingSerive.UpdateEventBooking(eventBooking);
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
        public IActionResult Delete(int id)
        {
            var result = _eventBookingSerive.DeleteEventBooking(id);
            if (!result)
            {
                return Json(new { success = false, message = "Failed to delete event booking." });
            }
            return Json(new { success = true, message = "Event booking deleted successfully." });
        }
        public IActionResult GetAllEventBookings()
        {
            var result = _eventBookingSerive.GetAllEventBookings();
            if (result == null || result.Count == 0)
            {
                return Json(result);
            }
            return Json(result);


        }
    }
}
