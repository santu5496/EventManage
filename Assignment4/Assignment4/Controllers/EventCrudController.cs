using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    public class EventCrudController : Controller
    {
        private readonly IEventCrudService _eventCrudService;
        public EventCrudController(IEventCrudService eventCrudService)
        {
            _eventCrudService = eventCrudService;
        }
        public IActionResult EventCrud()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddOrUpdateEvent(Events events)
        {
            if (events.eventId != 0)
            {
                var result = _eventCrudService.UpdateEvent(events);
                if (!result)
                {
                    return Json(new { success = false, message = "Failed to update event." });
                }
                return Json(new { success = true, message = "Event updated successfully." });
            }
            else
            {
                var result = _eventCrudService.AddEvent(events);
                if (!result)
                {
                    return Json(new { success = false, message = "Failed to add event." });
                }
                return Json(new { success = true, message = "Event added successfully." });
            }
        }
        public IActionResult Delete(int id)
        {
            var result = _eventCrudService.DeleteEvent(id);
            if (!result)
            {
                return Json(new { success = false, message = "Failed to delete event." });
            }
            return Json(new { success = true, message = "Event deleted successfully." });
        }
        public IActionResult GetAllEvents()
        {
            var result = _eventCrudService.GetAllEvents();
            if (result == null || result.Count == 0)
            {
                return Json(result);
            }
            return Json(result);

        }
    }
}
