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
        bool AddEventBooking(Events eventBooking);
        List<Events> GetAllEventBookings();
        bool UpdateEventBooking(Events eventBooking);
        bool DeleteEventBooking(int id);

    }
}
