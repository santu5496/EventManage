using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbOperation.Implementation
{
    public class EventBookingSerive : IEventBookingSerive
    {
        private readonly DbContextOptions<EventContext> _context;
        private readonly string _connectionString;

        public EventBookingSerive(string connectionString)
        {
            _connectionString = connectionString;
            _context = new DbContextOptionsBuilder<EventContext>()
                            .UseSqlServer(connectionString).Options;
            
            // Ensure database is created (wrapped in try-catch for offline testing)
            try
            {
                using (var db = new EventContext(_context))
                {
                    db.Database.EnsureCreated();
                }
            }
            catch
            {
                // Database not available - will work when deployed to bsite.net
            }
        }

        public bool AddDieselColumn()
        {
            // SQLite doesn't support ALTER TABLE ADD COLUMN in the same way
            // The column should be defined in the model instead
            return true;
        }

        public bool AddBandTypeColumn()
        {
            // SQLite doesn't support ALTER TABLE ADD COLUMN in the same way
            // The column should be defined in the model instead
            return true;
        }

        public bool AddNewBookingColumns()
        {
            return true;
        }

   

        public string LastError { get; private set; } = string.Empty;

        public bool AddBooking(Bookings bookings)
        {
            try
            {
                LastError = string.Empty;
                using (var db = new EventContext(_context))
                {
                    if (bookings.bookingId == 0)
                    {
                        bookings.createdDate = DateTime.UtcNow;
                        if (bookings.bookingDate.HasValue)
                            bookings.bookingDate = DateTime.SpecifyKind(bookings.bookingDate.Value, DateTimeKind.Utc);
                        if (bookings.fromDate.HasValue)
                            bookings.fromDate = DateTime.SpecifyKind(bookings.fromDate.Value, DateTimeKind.Utc);
                        if (bookings.toDate.HasValue)
                            bookings.toDate = DateTime.SpecifyKind(bookings.toDate.Value, DateTimeKind.Utc);
                        var existingUser = db.Users.FirstOrDefault();
                        if (existingUser == null)
                        {
                            LastError = "No users found in the system. Please create a user first.";
                            return false;
                        }
                        bookings.userId = existingUser.userId;

                        db.Bookings.Add(bookings);
                    }
                    else
                    {
                        var existing = db.Bookings.FirstOrDefault(b => b.bookingId == bookings.bookingId);
                        if (existing == null)
                        {
                            LastError = "Booking not found. It may have been deleted.";
                            return false;
                        }

                        if (bookings.eventId.HasValue)
                            existing.eventId = bookings.eventId;
                        if (bookings.bookingDate.HasValue)
                            existing.bookingDate = DateTime.SpecifyKind(bookings.bookingDate.Value, DateTimeKind.Utc);
                        if (bookings.fromDate.HasValue)
                            existing.fromDate = DateTime.SpecifyKind(bookings.fromDate.Value, DateTimeKind.Utc);
                        if (bookings.toDate.HasValue)
                            existing.toDate = DateTime.SpecifyKind(bookings.toDate.Value, DateTimeKind.Utc);
                        if (!string.IsNullOrEmpty(bookings.shiftType))
                            existing.shiftType = bookings.shiftType;
                        if (!string.IsNullOrEmpty(bookings.bookingStatus))
                            existing.bookingStatus = bookings.bookingStatus;
                        if (bookings.totalAmount.HasValue)
                            existing.totalAmount = bookings.totalAmount;
                        if (bookings.advancePayment.HasValue)
                            existing.advancePayment = bookings.advancePayment;
                        if (bookings.remainingPayment.HasValue)
                            existing.remainingPayment = bookings.remainingPayment;
                        if (!string.IsNullOrEmpty(bookings.paymentStatus))
                            existing.paymentStatus = bookings.paymentStatus;
                        if (!string.IsNullOrEmpty(bookings.customerName))
                            existing.customerName = bookings.customerName;
                        if (!string.IsNullOrEmpty(bookings.phoneNumber))
                            existing.phoneNumber = bookings.phoneNumber;
                        if (!string.IsNullOrEmpty(bookings.alternativeNumber))
                            existing.alternativeNumber = bookings.alternativeNumber;
                        if (!string.IsNullOrEmpty(bookings.address))
                            existing.address = bookings.address;
                        if (!string.IsNullOrEmpty(bookings.dateWiseShifts))
                            existing.dateWiseShifts = bookings.dateWiseShifts;
                        if (!string.IsNullOrEmpty(bookings.EventName))
                            existing.EventName = bookings.EventName;
                        if (bookings.diesel.HasValue)
                            existing.diesel = bookings.diesel;
                        if (!string.IsNullOrEmpty(bookings.bandType))
                            existing.bandType = bookings.bandType;
                    }

                    var changes = db.SaveChanges();
                    if (changes == 0)
                    {
                        LastError = "No changes were made to the booking.";
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException?.Message ?? ex.Message;
                LastError = $"Database error: {innerMsg}";
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Error: {ex.InnerException.Message}");
                return false;
            }
        }

        public string GetLastError()
        {
            return LastError;
        }

        public bool AddEventNameColumn()
        {
            // Schema managed by EF Core model
            return true;
        }

        public bool AddDateWiseShiftsColumn()
        {
            // Schema managed by EF Core model
            return true;
        }

        public List<Bookings> GetAllBookings()
        {


            
            using (var db = new EventContext(_context))
            { 
               
                return db.Bookings.ToList();
            }
            
        }
        public void DeleteAllBookings()
        {
            using (var db = new EventContext(_context))
            {
                // Fetch all bookings
                var all = db.Bookings.ToList();

                // Remove them in one shot
                db.Bookings.RemoveRange(all);

                // Persist the delete
                db.SaveChanges();
            }
        }

        public bool UpdateBooking(Bookings bookings)
        {
            using (var db = new EventContext(_context))
            {
                var existing = db.Bookings.FirstOrDefault(b => b.bookingId == bookings.bookingId);
                if (existing == null)
                {
                    return false;
                }

                if (bookings.eventId.HasValue)
                    existing.eventId = bookings.eventId;
                if (bookings.bookingDate.HasValue)
                    existing.bookingDate = DateTime.SpecifyKind(bookings.bookingDate.Value, DateTimeKind.Utc);
                if (bookings.fromDate.HasValue)
                    existing.fromDate = DateTime.SpecifyKind(bookings.fromDate.Value, DateTimeKind.Utc);
                if (bookings.toDate.HasValue)
                    existing.toDate = DateTime.SpecifyKind(bookings.toDate.Value, DateTimeKind.Utc);
                if (!string.IsNullOrEmpty(bookings.shiftType))
                    existing.shiftType = bookings.shiftType;
                if (!string.IsNullOrEmpty(bookings.bookingStatus))
                    existing.bookingStatus = bookings.bookingStatus;
                if (bookings.totalAmount.HasValue)
                    existing.totalAmount = bookings.totalAmount;
                if (bookings.advancePayment.HasValue)
                    existing.advancePayment = bookings.advancePayment;
                if (bookings.remainingPayment.HasValue)
                    existing.remainingPayment = bookings.remainingPayment;
                if (!string.IsNullOrEmpty(bookings.paymentStatus))
                    existing.paymentStatus = bookings.paymentStatus;
                if (!string.IsNullOrEmpty(bookings.customerName))
                    existing.customerName = bookings.customerName;
                if (!string.IsNullOrEmpty(bookings.phoneNumber))
                    existing.phoneNumber = bookings.phoneNumber;
                if (!string.IsNullOrEmpty(bookings.alternativeNumber))
                    existing.alternativeNumber = bookings.alternativeNumber;
                if (!string.IsNullOrEmpty(bookings.address))
                    existing.address = bookings.address;
                if (!string.IsNullOrEmpty(bookings.EventName))
                    existing.EventName = bookings.EventName;
                if (bookings.diesel.HasValue)
                    existing.diesel = bookings.diesel;
                if (!string.IsNullOrEmpty(bookings.bandType))
                    existing.bandType = bookings.bandType;
                if (!string.IsNullOrEmpty(bookings.dateWiseShifts))
                    existing.dateWiseShifts = bookings.dateWiseShifts;

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

        public bool AddEventBooking(Bookings eventBooking)
        {
            try
            {
                using (var db = new EventContext(_context))
                {
                    eventBooking.eventId = 3;
                    eventBooking.userId = 3;
                    eventBooking.bookingDate = DateTime.UtcNow;
                    eventBooking.createdDate = DateTime.UtcNow;
                    if (eventBooking.fromDate.HasValue)
                        eventBooking.fromDate = DateTime.SpecifyKind(eventBooking.fromDate.Value, DateTimeKind.Utc);
                    if (eventBooking.toDate.HasValue)
                        eventBooking.toDate = DateTime.SpecifyKind(eventBooking.toDate.Value, DateTimeKind.Utc);
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
                var existing = db.Bookings.FirstOrDefault(b => b.bookingId == eventBooking.bookingId);
                if (existing == null)
                {
                    return false;
                }

                if (eventBooking.eventId.HasValue)
                    existing.eventId = eventBooking.eventId;
                if (eventBooking.bookingDate.HasValue)
                    existing.bookingDate = DateTime.SpecifyKind(eventBooking.bookingDate.Value, DateTimeKind.Utc);
                if (eventBooking.fromDate.HasValue)
                    existing.fromDate = DateTime.SpecifyKind(eventBooking.fromDate.Value, DateTimeKind.Utc);
                if (eventBooking.toDate.HasValue)
                    existing.toDate = DateTime.SpecifyKind(eventBooking.toDate.Value, DateTimeKind.Utc);
                if (!string.IsNullOrEmpty(eventBooking.shiftType))
                    existing.shiftType = eventBooking.shiftType;
                if (!string.IsNullOrEmpty(eventBooking.bookingStatus))
                    existing.bookingStatus = eventBooking.bookingStatus;
                if (eventBooking.totalAmount.HasValue)
                    existing.totalAmount = eventBooking.totalAmount;
                if (eventBooking.advancePayment.HasValue)
                    existing.advancePayment = eventBooking.advancePayment;
                if (eventBooking.remainingPayment.HasValue)
                    existing.remainingPayment = eventBooking.remainingPayment;
                if (!string.IsNullOrEmpty(eventBooking.paymentStatus))
                    existing.paymentStatus = eventBooking.paymentStatus;
                if (!string.IsNullOrEmpty(eventBooking.customerName))
                    existing.customerName = eventBooking.customerName;
                if (!string.IsNullOrEmpty(eventBooking.phoneNumber))
                    existing.phoneNumber = eventBooking.phoneNumber;
                if (!string.IsNullOrEmpty(eventBooking.alternativeNumber))
                    existing.alternativeNumber = eventBooking.alternativeNumber;
                if (!string.IsNullOrEmpty(eventBooking.address))
                    existing.address = eventBooking.address;
                if (!string.IsNullOrEmpty(eventBooking.EventName))
                    existing.EventName = eventBooking.EventName;
                if (eventBooking.diesel.HasValue)
                    existing.diesel = eventBooking.diesel;
                if (!string.IsNullOrEmpty(eventBooking.bandType))
                    existing.bandType = eventBooking.bandType;
                if (!string.IsNullOrEmpty(eventBooking.dateWiseShifts))
                    existing.dateWiseShifts = eventBooking.dateWiseShifts;

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
