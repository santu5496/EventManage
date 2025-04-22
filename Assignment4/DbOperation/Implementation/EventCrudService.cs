using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbOperation.Implementation
{
    public class EventCrudService: IEventCrudService
    {
        private readonly DbContextOptions<EventContext> _context;

        public EventCrudService(string context)
        {
            _context = new DbContextOptionsBuilder<EventContext>().UseSqlServer(context).Options;
        }
        public bool AddEvent(Events events)
        {
            try
            {
                using (var db = new EventContext(_context))
                {
                    events.createdDate = DateTime.Now;
                    db.Events.Add(events);
                    return db.SaveChanges() > 0;  // This returns `true` if rows are affected, `false` if no rows are affected
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error: {ex.Message}");
                return false;  // Return false if any error occurs
            }
        }
        public List<Events> GetAllEvents()
        {
            using (var db = new EventContext(_context))
            {
                return db.Events.ToList();
            }
        }
        public bool UpdateEvent(Events events)
        {
            using (var db = new EventContext(_context))
            {
                events.createdDate = DateTime.Now;
                db.Events.Update(events);
                return db.SaveChanges() > 0;
            }
        }
        public bool DeleteEvent(int id)
        {
            using (var db = new EventContext(_context))
            {
                var eventToDelete = db.Events.Find(id);
                if (eventToDelete != null)
                {
                    db.Events.Remove(eventToDelete);
                    return db.SaveChanges() > 0;
                }
                return false; // Event not found
            }
        }

    }
}
