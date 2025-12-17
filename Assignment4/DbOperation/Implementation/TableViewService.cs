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
    public class TableViewService:ITableViewService
    {
        private readonly DbContextOptions<EventContext> _context;
        public TableViewService(string context)
        {
            _context = new DbContextOptionsBuilder<EventContext>().UseSqlite(context).Options;
        }
    }
}
