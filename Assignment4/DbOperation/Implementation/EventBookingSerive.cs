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
    public class EventBookingSerive : IEventBookingSerive
    {
        private readonly DbContextOptions<EventContext> _context;
        public EventBookingSerive(string context)
        {
            _context = new DbContextOptionsBuilder<EventContext>().UseSqlServer(context).Options;
        }

        public bool AddBooking(Bookings bookings)
        {
            try
            {
                using (var db = new EventContext(_context))
                {
                    bookings.createdDate = DateTime.Now;
                    bookings.userId = 4;
                    db.Bookings.Add(bookings);
                  
                    return db.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public List<Bookings> GetAllBookings()
        {
            using (var db = new EventContext(_context))
            {
                return db.Bookings.ToList();
            }
        }

        public bool UpdateBooking(Bookings bookings)
        {
            using (var db = new EventContext(_context))
            {
                db.Bookings.Update(bookings);
                return db.SaveChanges() > 0;
            }
        }

        public bool DeleteBooking(int id)
        {
            using (var db = new EventContext(_context))
            {
                var bookingToDelete = db.Bookings.Find(id);
                if (bookingToDelete != null)
                {
                    db.Bookings.Remove(bookingToDelete);
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }

        // Implementing missing interface members
        public bool AddEventBooking(Bookings eventBooking)
        {
            try
            {
                using (var db = new EventContext(_context))
                {
                    eventBooking.eventId = 3;
                    eventBooking.userId = 3;
                    eventBooking.bookingDate = DateTime.Now;
                    eventBooking.createdDate = DateTime.Now;
                    db.Bookings.Add(eventBooking);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public List<Events> GetAllEventBookings()
        {
            using (var db = new EventContext(_context))
            {
                return db.Events.ToList();
            }
        }

        public bool UpdateEventBooking(Bookings eventBooking)
        {
            using (var db = new EventContext(_context))
            {
                db.Bookings.Update(eventBooking);
                
                return db.SaveChanges() > 0;
            }
        }

        public bool DeleteEventBooking(int id)
        {
            using (var db = new EventContext(_context))
            {
                var eventToDelete = db.Events.Find(id);
                if (eventToDelete != null)
                {
                    db.Events.Remove(eventToDelete);
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }
        public List<Bookings> GetBookingByID(int id)
        {
            using (var db = new EventContext(_context))
            {
                var selectedBooking = db.Bookings.FirstOrDefault(x => x.bookingId == id);
                return selectedBooking != null ? new List<Bookings> { selectedBooking } : new List<Bookings>();
            }
        }




    }
}
