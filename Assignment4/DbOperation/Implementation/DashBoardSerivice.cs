using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.EntityFrameworkCore;


namespace DbOperation.Implementation
{
    public class DashBoardSerivice:IDashBoard
    {

        private readonly DbContextOptions<EventContext> _context;
        public DashBoardSerivice(string context)
        {
            _context = new DbContextOptionsBuilder<EventContext>().UseSqlServer(context).Options;
        }
    
            // Get total booking count for dashboard
            public int GetTotalBookingsCount()
            {
                using (var db = new EventContext(_context))
                {
                    return db.Bookings.Count();
                }
            }

            // Get total revenue
            public decimal GetTotalRevenue()
            {
                using (var db = new EventContext(_context))
                {
                    return (decimal)db.Bookings
                        .Where(b => b.bookingStatus == "Confirmed" || b.bookingStatus == "Completed")
                        .Sum(b => b.totalAmount);
                }
            }

            // Get pending bookings count
            public int GetPendingBookingsCount()
            {
                using (var db = new EventContext(_context))
                {
                    return db.Bookings.Count(b => b.bookingStatus == "Pending");
                }
            }

            // Get pending payments amount
            public decimal GetPendingPaymentsAmount()
            {
                using (var db = new EventContext(_context))
                {
                    return (decimal)db.Bookings
                        .Where(b => b.paymentStatus == "Unpaid" || b.paymentStatus == "Partial")
                        .Sum(b => b.totalAmount);
                }
            }

        // Get recent bookings for the table
        public List<BookingsViewModel> GetRecentBookings(int count = 5)
        {
            using (var db = new EventContext(_context))
            {
                return db.Bookings
                    .OrderByDescending(b => b.bookingDate)
                    .Take(count)
                    .Join(db.Events,
                        booking => booking.eventId,
                        evt => evt.eventId,
                        (booking, evt) => new BookingsViewModel
                        {
                            BookingId = booking.bookingId,
                            CustomerName = booking.customerName,
                            EventName = evt.eventName,
                            EventDate = booking.bookingDate,
                            Amount = booking.totalAmount ?? 0,
                            Status = booking.bookingStatus,
                            PaymentStatus = booking.paymentStatus
                        })
                    .ToList();
            }
        }

        // Get booking trends for chart (monthly booking counts)
        public Dictionary<string, int> GetMonthlyBookingTrends(int year)
            {
                using (var db = new EventContext(_context))
                {
                    var bookingsByMonth = db.Bookings
    .Where(b => b.createdDate.HasValue && b.createdDate.Value.Year == year)
                        .GroupBy(b => b.createdDate.Value.Month)
                        .Select(g => new { Month = g.Key, Count = g.Count() })
                        .ToDictionary(x => GetMonthName(x.Month), x => x.Count);

                    // Ensure all months are represented
                    for (int i = 1; i <= 12; i++)
                    {
                        string monthName = GetMonthName(i);
                        if (!bookingsByMonth.ContainsKey(monthName))
                        {
                            bookingsByMonth[monthName] = 0;
                        }
                    }

                    return bookingsByMonth;
                }
            }

            // Get monthly revenue data for chart
            public Dictionary<string, decimal> GetMonthlyRevenueTrends(int year)
            {
                using (var db = new EventContext(_context))
                {
                    var revenueByMonth = db.Bookings
                        .Where(b => b.bookingDate.Value.Year == year &&
                               (b.bookingStatus == "Confirmed" || b.bookingStatus == "Completed"))
                        .GroupBy(b => b.bookingDate.Value.Month)
                        .Select(g => new { Month = g.Key, Revenue = g.Sum(b => b.totalAmount) ?? 0 }) // Ensure null values are handled
                        .ToDictionary(x => GetMonthName(x.Month), x => x.Revenue);

                    // Ensure all months are represented
                    for (int i = 1; i <= 12; i++)
                    {
                        string monthName = GetMonthName(i);
                        if (!revenueByMonth.ContainsKey(monthName))
                        {
                            revenueByMonth[monthName] = 0;
                        }
                    }

                    return revenueByMonth;
                }
            }

            // Get booking bookingStatus distribution for pie chart
            public Dictionary<string, int> GetBookingbookingStatusDistribution()
            {
                using (var db = new EventContext(_context))
                {
                    return db.Bookings
                        .GroupBy(b => b.bookingStatus)
                        .Select(g => new { Status = g.Key, Count = g.Count() })
                        .ToDictionary(x => x.Status, x => x.Count);
                }
            }

            // Get events by category for chart
            public Dictionary<string, int> GetEventsByCategory()
            {
                using (var db = new EventContext(_context))
                {
                    return db.Events
                        .GroupBy(e => e.eventName)
                        .Select(g => new { Category = g.Key, Count = g.Count() })
                        .ToDictionary(x => x.Category, x => x.Count);
                }
            }

            // Get calendar events for specific month
            public List<CalendarEventViewModel> GetCalendarEvents(int year, int month)
            {
                using (var db = new EventContext(_context))
                {
                    DateTime startDate = new DateTime(year, month, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    return db.Bookings
                        .Where(b => b.bookingDate >= startDate && b.bookingDate <= endDate)
                        .Join(db.Events,
                            booking => booking.eventId,
                            events => events.userId,
                            (booking, events) => new { Booking = booking, Event = events })
                        .Join(db.Users,
                            combined => combined.Booking.userId,
                            user => user.userId,
                            (combined, user) => new CalendarEventViewModel
                            {
                                BookingId = combined.Booking.bookingId,
                                EventId = combined.Event.eventId,
                                EventName = combined.Event.eventName,
                                CustomerName = $"{user.fullName} {user.fullName}",
                                EventDate = (DateTime)combined.Booking.bookingDate,
                                Amount = (decimal)combined.Booking.totalAmount,
                                Status = combined.Booking.bookingStatus
                            })
                        .ToList();
                }
            }

            // Get top venues by booking count and revenue
            public List<VenueMetricsViewModel> GetTopVenues(int count = 5)
            {
                using (var db = new EventContext(_context))
                {
                    return db.Bookings
                        .Join(db.Events,
                            booking => booking.eventId,
                            events => events.userId,
                            (booking, events) => new { Booking = booking, Event = events })
                        .Join(db.Events,
                            combined => combined.Event.eventId,
                            venue => venue.eventId,
                            (combined, venue) => new { combined.Booking, combined.Event, Venue = venue })
                        .GroupBy(x => new { x.Venue.eventId, x.Venue.eventName })
                        .Select(g => new VenueMetricsViewModel
                        {
                            VenueId = g.Key.eventId,
                            VenueName = g.Key.eventName,
                            EventCount = g.Count(),
                            TotalRevenue = (decimal)g.Sum(x => x.Booking.totalAmount)
                        })
                        .OrderByDescending(x => x.TotalRevenue)
                        .Take(count)
                        .ToList();
                }
            }

            // Get upcoming venue bookings
            public List<UpcomingVenueBookingViewModel> GetUpcomingVenueBookings(int count = 3)
            {
                using (var db = new EventContext(_context))
                {
                    return db.Bookings
                        .Where(b => b.bookingDate > DateTime.Now)
                        .OrderBy(b => b.bookingDate)
                        .Join(db.Events,
                            booking => booking.eventId,
                            events => events.eventId,
                            (booking, events) => new { Booking = booking, Event = events })
                        .Join(db.Events,
                            combined => combined.Event.eventId,
                            venue => venue.eventId,
                            (combined, venue) => new UpcomingVenueBookingViewModel
                            {
                                BookingId = combined.Booking.bookingId,
                                VenueId = venue.eventId,
                                VenueName = venue.eventName,
                                EventDate = (DateTime)combined.Booking.bookingDate,
                                Status = combined.Booking.bookingStatus
                            })
                        .Take(count)
                        .ToList();
                }
            }

            // Get recent transactions for payment section
            public List<RecentTransactionViewModel> GetRecentTransactions(int count = 3)
            {
                //using (var db = new EventContext(_context))
                //{
                //    return db.Payments
                //        .OrderByDescending(p => p.paymentDate)
                //        .Take(count)
                //        .Join(db.Bookings,
                //            payment => payment.bookingId,
                //            booking => booking.bookingId,
                //            (payment, booking) => new { Payment = payment, Booking = booking })
                //        .Join(db.Events,
                //            combined => combined.Booking.eventId,
                //            events => events.eventId,
                //            (combined, events) => new { combined.Payment, combined.Booking, Event = events })
                //        .Join(db.Users,
                //            combined => combined.Booking.userId,
                //            user => user.userId,
                //            (combined, user) => new RecentTransactionViewModel
                //            {
                //                PaymentId = combined.Payment.id,
                //                BookingId = combined.Booking.id,
                //                CustomerName = $"{user.firstName} {user.lastName}",
                //                CustomerInitials = $"{user.firstName[0]}{user.lastName[0]}",
                //                EventName = combined.Event.eventName,
                //                EventDate = combined.Booking.eventDate,
                //                Amount = combined.Payment.amount,
                //                PaymentStatus = combined.Booking.paymentStatus
                //            })
                //        .ToList();
                //}
                return null;
            }

            // Get dashboard metrics percentage changes (compared to previous period)
            public DashboardMetricsViewModel GetDashboardMetrics()
            {
                using (var db = new EventContext(_context))
                {
                    // Current period (this month)
                    DateTime currentPeriodStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime currentPeriodEnd = currentPeriodStart.AddMonths(1).AddDays(-1);

                    // Previous period (last month)
                    DateTime previousPeriodStart = currentPeriodStart.AddMonths(-1);
                    DateTime previousPeriodEnd = currentPeriodStart.AddDays(-1);

                    // Calculate current metrics
                    int currentBookings = db.Bookings.Count(b => b.createdDate >= currentPeriodStart && b.createdDate <= currentPeriodEnd);
                    decimal currentRevenue = (decimal)db.Bookings
                        .Where(b => b.createdDate >= currentPeriodStart && b.createdDate <= currentPeriodEnd)
                        .Sum(b => b.totalAmount);
                    int currentPendingBookings = db.Bookings
                        .Count(b => b.bookingStatus == "Pending" && b.createdDate >= currentPeriodStart && b.createdDate <= currentPeriodEnd);
                    decimal currentPendingPayments = (decimal)db.Bookings
                        .Where(b => (b.paymentStatus == "Unpaid" || b.paymentStatus == "Partial") &&
                               b.createdDate >= currentPeriodStart && b.createdDate <= currentPeriodEnd)
                        .Sum(b => b.totalAmount);

                    // Calculate previous metrics
                    int previousBookings = db.Bookings.Count(b => b.createdDate >= previousPeriodStart && b.createdDate <= previousPeriodEnd);
                    decimal previousRevenue = (decimal)db.Bookings
                        .Where(b => b.createdDate >= previousPeriodStart && b.createdDate <= previousPeriodEnd)
                        .Sum(b => b.totalAmount);
                    int previousPendingBookings = db.Bookings
                        .Count(b => b.bookingStatus == "Pending" && b.createdDate >= previousPeriodStart && b.createdDate <= previousPeriodEnd);
                    decimal previousPendingPayments = (decimal)db.Bookings
                        .Where(b => (b.paymentStatus == "Unpaid" || b.paymentStatus == "Partial") &&
                               b.createdDate >= previousPeriodStart && b.createdDate <= previousPeriodEnd)
                        .Sum(b => b.totalAmount);

                    // Calculate percentage changes
                    decimal bookingsChange = CalculatePercentageChange(previousBookings, currentBookings);
                    decimal revenueChange = CalculatePercentageChange(previousRevenue, currentRevenue);
                    decimal pendingBookingsChange = CalculatePercentageChange(previousPendingBookings, currentPendingBookings);
                    decimal pendingPaymentsChange = CalculatePercentageChange(previousPendingPayments, currentPendingPayments);

                    return new DashboardMetricsViewModel
                    {
                        TotalBookings = currentBookings,
                        TotalRevenue = currentRevenue,
                        PendingBookings = currentPendingBookings,
                        PendingPayments = currentPendingPayments,
                        BookingsChangePercentage = bookingsChange,
                        RevenueChangePercentage = revenueChange,
                        PendingBookingsChangePercentage = pendingBookingsChange,
                        PendingPaymentsChangePercentage = pendingPaymentsChange
                    };
                }
            }

            // Get payment collection status
            public PaymentCollectionViewModel GetPaymentCollectionStatus()
            {
                using (var db = new EventContext(_context))
                {
                    decimal collectedAmount = (decimal)db.Bookings
                        .Where(b => b.paymentStatus == "Paid")
                        .Sum(b => b.totalAmount);

                    decimal pendingAmount = (decimal)db.Bookings
                        .Where(b => b.paymentStatus == "Unpaid" || b.paymentStatus == "Partial")
                        .Sum(b => b.totalAmount);

                    decimal totalAmount = collectedAmount + pendingAmount;
                    decimal collectionPercentage = totalAmount > 0 ? (collectedAmount / totalAmount) * 100 : 0;

                    return new PaymentCollectionViewModel
                    {
                        CollectedAmount = collectedAmount,
                        PendingAmount = pendingAmount,
                        CollectionPercentage = collectionPercentage
                    };
                }
            }
         

            #region Helper Methods
            private string GetMonthName(int month)
            {
                return new DateTime(2020, month, 1).ToString("MMM");
            }

            private decimal CalculatePercentageChange(decimal oldValue, decimal newValue)
            {
                if (oldValue == 0)
                    return newValue > 0 ? 100 : 0;

                return Math.Round(((newValue - oldValue) / oldValue) * 100, 1);
            }
            #endregion
        }

        #region View Models
        public class BookingsViewModel
        {
            public int BookingId { get; set; }
            public string CustomerName { get; set; }
            public string EventName { get; set; }
            public DateTime? EventDate { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
            public string PaymentStatus { get; set; }
        }

        public class CalendarEventViewModel
        {
            public int BookingId { get; set; }
            public int EventId { get; set; }
            public string EventName { get; set; }
            public string CustomerName { get; set; }
            public DateTime EventDate { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
        }

        public class VenueMetricsViewModel
        {
            public int VenueId { get; set; }
            public string VenueName { get; set; }
            public int EventCount { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        public class UpcomingVenueBookingViewModel
        {
            public int BookingId { get; set; }
            public int VenueId { get; set; }
            public string VenueName { get; set; }
            public DateTime EventDate { get; set; }
            public string Status { get; set; }
        }

        public class RecentTransactionViewModel
        {
            public int PaymentId { get; set; }
            public int BookingId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerInitials { get; set; }
            public string EventName { get; set; }
            public DateTime EventDate { get; set; }
            public decimal Amount { get; set; }
            public string PaymentStatus { get; set; }
        }

        public class DashboardMetricsViewModel
        {
            public int TotalBookings { get; set; }
            public decimal TotalRevenue { get; set; }
            public int PendingBookings { get; set; }
            public decimal PendingPayments { get; set; }
            public decimal BookingsChangePercentage { get; set; }
            public decimal RevenueChangePercentage { get; set; }
            public decimal PendingBookingsChangePercentage { get; set; }
            public decimal PendingPaymentsChangePercentage { get; set; }
        }

        public class PaymentCollectionViewModel
        {
            public decimal CollectedAmount { get; set; }
            public decimal PendingAmount { get; set; }
            public decimal CollectionPercentage { get; set; }
        }
        
    }





#endregion
   

