using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbOperation.Interface
{
    public interface IEventCrudService
    {
        bool AddEvent(Models.Events events);
        List<Models.Events> GetAllEvents();
        bool UpdateEvent(Models.Events events);
        bool DeleteEvent(int id);

    }
}
