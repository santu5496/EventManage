using DbOperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbOperation.Interface
{
    public interface IEventBookingSerive
    {
        bool UpdateBooking(Bookings bookings);
        bool DeleteBooking(int id);
        bool AddEventBooking(Bookings eventBooking);
        List<Bookings> GetAllBookings();
        List<Bookings> GetBookingByID(int id);
        bool AddBooking(Bookings bookings);



    }
}
