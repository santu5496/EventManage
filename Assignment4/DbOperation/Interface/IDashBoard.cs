using DbOperation.Implementation;
using System;
using System.Collections.Generic;

namespace DbOperation.Interface
{
    public interface IDashBoard
    {
        // Dashboard Data Methods
        int GetTotalBookingsCount();
        decimal GetTotalRevenue();
        int GetPendingBookingsCount();
        decimal GetPendingPaymentsAmount();
        List<BookingsViewModel> GetRecentBookings(int count = 5);
        Dictionary<string, int> GetMonthlyBookingTrends(int year);
        Dictionary<string, decimal> GetMonthlyRevenueTrends(int year);
        Dictionary<string, int> GetBookingbookingStatusDistribution();
        Dictionary<string, int> GetEventsByCategory();
        List<CalendarEventViewModel> GetCalendarEvents(int year, int month);
        List<VenueMetricsViewModel> GetTopVenues(int count = 5);
        List<UpcomingVenueBookingViewModel> GetUpcomingVenueBookings(int count = 3);
        List<RecentTransactionViewModel> GetRecentTransactions(int count = 3);
        DashboardMetricsViewModel GetDashboardMetrics();
        PaymentCollectionViewModel GetPaymentCollectionStatus();
    }
}
