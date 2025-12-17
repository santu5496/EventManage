using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.Data.SqlClient;
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
            //var a = AddDateWiseShiftsColumn();
            AddNewBookingColumns();



        }
        // Add these methods to your EventBookingService class

        public bool AddDieselColumn()
        {
            const string alterQuery = @"
        IF NOT EXISTS (
            SELECT * 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'Bookings' 
              AND COLUMN_NAME = 'diesel'
        )
        BEGIN
            ALTER TABLE Bookings ADD diesel DECIMAL(18,2) NULL;
        END";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(alterQuery, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Column 'diesel' checked/added successfully.");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool AddBandTypeColumn()
        {
            const string alterQuery = @"
        IF NOT EXISTS (
            SELECT * 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'Bookings' 
              AND COLUMN_NAME = 'bandType'
        )
        BEGIN
            ALTER TABLE Bookings ADD bandType NVARCHAR(50) NULL;
        END";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(alterQuery, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Column 'bandType' checked/added successfully.");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        // Method to add both columns at once
        public bool AddNewBookingColumns()
        {
            bool dieselAdded = AddDieselColumn();
            bool bandTypeAdded = AddBandTypeColumn();

            return dieselAdded && bandTypeAdded;
        }

   

        public bool AddBooking(Bookings bookings)
        {
            try
            {
                using (var db = new EventContext(_context))
                {
                    if (bookings.bookingId == 0)
                    {
                        bookings.createdDate = DateTime.Now;
                        var existing = db.Users.FirstOrDefault();
                        var userid = existing.userId;

                        bookings.userId = userid;

                        db.Bookings.Add(bookings);
                        
                    }
                    else
                    {
                        var existing = db.Bookings.FirstOrDefault(b => b.bookingId == bookings.bookingId);
                        if (existing != null)
                        {
                            // Update fields
                            existing.eventId = bookings.eventId;
                            existing.bookingDate = bookings.bookingDate;
                            existing.fromDate = bookings.fromDate;
                            existing.toDate = bookings.toDate;
                            existing.shiftType = bookings.shiftType;
                            existing.bookingStatus = bookings.bookingStatus;
                            existing.totalAmount = bookings.totalAmount;
                            existing.advancePayment = bookings.advancePayment;
                            existing.remainingPayment = bookings.remainingPayment;
                            existing.paymentStatus = bookings.paymentStatus;
                            existing.customerName = bookings.customerName;
                            existing.phoneNumber = bookings.phoneNumber;
                            existing.alternativeNumber = bookings.alternativeNumber;
                            existing.address = bookings.address;
                            existing.dateWiseShifts = bookings.dateWiseShifts;
                            db.Bookings.Update(existing);
                        }
                    }

                    return db.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool AddEventNameColumn()
        {
            const string alterQuery = @"
        IF NOT EXISTS (
            SELECT * 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'Bookings' 
              AND COLUMN_NAME = 'EventName'
        )
        BEGIN
            ALTER TABLE Bookings ADD EventName NVARCHAR(100);
        END";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(alterQuery, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Column 'EventName' checked/added successfully.");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool AddDateWiseShiftsColumn()
        {
            const string alterQuery = @"
                IF NOT EXISTS (
                    SELECT * 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Bookings' 
                      AND COLUMN_NAME = 'EventName'
                )
                BEGIN
                    ALTER TABLE Bookings ADD dateWiseShifts NVARCHAR(100);
                END";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(alterQuery, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Column 'dateWiseShifts' checked/added successfully.");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
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
