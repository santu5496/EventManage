using DbOperation.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Assignment4.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IDashBoard _dashBoardService;

        public DashBoardController(IDashBoard dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }
        public IActionResult DashBoard()
        {
            return View();
        }

        // Get total bookings count
        public IActionResult GetTotalBookingsCount()
        {
            var count = _dashBoardService.GetTotalBookingsCount();
            return Json(new { success = true, totalBookings = count });
        }

        // Get total revenue
        public IActionResult GetTotalRevenue()
        {
            var revenue = _dashBoardService.GetTotalRevenue();
            return Json(new { success = true, totalRevenue = revenue });
        }

        // Get pending bookings count
        public IActionResult GetPendingBookingsCount()
        {
            var count = _dashBoardService.GetPendingBookingsCount();
            return Json(new { success = true, pendingBookings = count });
        }

        // Get pending payments amount
        public IActionResult GetPendingPaymentsAmount()
        {
            var amount = _dashBoardService.GetPendingPaymentsAmount();
            return Json(new { success = true, pendingPayments = amount });
        }

        // Get recent bookings
        public IActionResult GetRecentBookings(int count = 5)
        {
            var bookings = _dashBoardService.GetRecentBookings(count);
            return Json(new { success = true, recentBookings = bookings });
        }

        // Get monthly booking trends
        public IActionResult GetMonthlyBookingTrends(int year)
        {
            var trends = _dashBoardService.GetMonthlyBookingTrends(year);
            return Json(new { success = true, monthlyBookingTrends = trends });
        }

        // Get monthly revenue trends
        public IActionResult GetMonthlyRevenueTrends(int year)
        {
            var trends = _dashBoardService.GetMonthlyRevenueTrends(year);
            return Json(new { success = true, monthlyRevenueTrends = trends });
        }

        // Get booking status distribution
        public IActionResult GetBookingStatusDistribution()
        {
            var distribution = _dashBoardService.GetBookingbookingStatusDistribution();
            return Json(new { success = true, bookingStatusDistribution = distribution });
        }

        // Get events by category
        public IActionResult GetEventsByCategory()
        {
            var events = _dashBoardService.GetEventsByCategory();
            return Json(new { success = true, eventsByCategory = events });
        }

        // Get calendar events for a specific month
        public IActionResult GetCalendarEvents(int year, int month)
        {
            var events = _dashBoardService.GetCalendarEvents(year, month);
            return Json(new { success = true, calendarEvents = events });
        }

        // Get top venues
        public IActionResult GetTopVenues(int count = 5)
        {
            var venues = _dashBoardService.GetTopVenues(count);
            return Json(new { success = true, topVenues = venues });
        }

        // Get upcoming venue bookings
        public IActionResult GetUpcomingVenueBookings(int count = 3)
        {
            var bookings = _dashBoardService.GetUpcomingVenueBookings(count);
            return Json(new { success = true, upcomingVenueBookings = bookings });
        }

        // Get recent transactions
        public IActionResult GetRecentTransactions(int count = 3)
        {
            var transactions = _dashBoardService.GetRecentTransactions(count);
            return Json(new { success = true, recentTransactions = transactions });
        }

        // Get dashboard metrics
        public IActionResult GetDashboardMetrics()
        {
            var metrics = _dashBoardService.GetDashboardMetrics();
            return Json(new { success = true, dashboardMetrics = metrics });
        }

        // Get payment collection status
        public IActionResult GetPaymentCollectionStatus()
        {
            var status = _dashBoardService.GetPaymentCollectionStatus();
            return Json(new { success = true, paymentCollectionStatus = status });
        }
    }
}
