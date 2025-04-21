using DbOperation.Implementation;
using DbOperation.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Assignment4.Controllers
{

    public class ReportsController : Controller
    {
        private readonly IReportService _dbConn;

        public ReportsController(IReportService db)
        {
            _dbConn = db;
        }
        public IActionResult DailyReport()
        {
            ViewBag.TotalSales = JsonConvert.SerializeObject(_dbConn.GetTotalSales(DateTime.Now));
            ViewBag.TotalProduced = JsonConvert.SerializeObject(_dbConn.GetTotalProduced(DateTime.Now));
            ViewBag.TotalPurchased = JsonConvert.SerializeObject(_dbConn.GetTotalPurchased(DateTime.Now));

            return View();
        }

        //public IActionResult exporttopdf(DateTime reportdate)
        //{
        //    var totalsales = _dbConn.GetTotalSales(reportdate);
        //    var totalproduced = _dbConn.GetTotalProduced(reportdate);
        //    var totalpurchased = _dbConn.GetTotalPurchased(reportdate);

        //    var pdfdata = _reportservice.generatepdfreport(reportdate, totalsales, totalproduced, totalpurchased);

        //    return file(pdfdata, "application/pdf", $"dailyreport_{reportdate:yyyy-mm-dd}.pdf");
        //}
    }
}
