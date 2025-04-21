using DbOperation.Interface;
using DbOperation.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using ZXing.OneD;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
namespace DbOperation.Implementation
{
    public class ReportService : IReportService
    {
        private readonly DbContextOptions<Assignment4Context> _dbConn;

        public ReportService(string dbConn)
        {
            _dbConn = new DbContextOptionsBuilder<Assignment4Context>().UseSqlServer(dbConn).Options;
        }
        public List<dynamic> GetTotalSales(DateTime reportDate)
        {
            var db = new Assignment4Context(_dbConn);
            var result = (from boi in db.BillOrderItems
                    join inv in db.Inventory on boi.fkItemId equals inv.itemID
                    join iItem in db.InventoryItems on boi.fkItemId equals iItem.itemId
                    join b in db.Billing on boi.fkBillOrderId equals b.billId
                    where b.billDate.Date == reportDate.Date
                    group boi by iItem.itemName into sales
                    select new
                    {
                        ItemName = sales.Key.ToString(),
                        TotalSold = sales.Sum(x => x.quantity)
                    }).ToList<dynamic>();

            return result;
        }

        public List<dynamic> GetTotalProduced(DateTime reportDate)
        {
            var db = new Assignment4Context(_dbConn);
            var result = (from bl in db.BakedCookedLogs
                    join inv in db.Inventory on bl.fkItemId equals inv.itemID
                    join iItem in db.InventoryItems on bl.fkItemId equals iItem.itemId
                    where bl.createdDate.Date == reportDate.Date
                    group bl by iItem.itemName into produced
                    select new
                    {
                        ItemName = produced.Key.ToString(),
                        TotalProduced = produced.Sum(x => x.actualQuantity)
                    }).ToList<dynamic>();

            return result;
        }

        public List<dynamic> GetTotalPurchased(DateTime reportDate)
        {
            var db = new Assignment4Context(_dbConn);
            var result = (from pd in db.PurchaseDetails
                          join rml in db.RawMaterialLogs on pd.fkRawMaterialId equals rml.logId
                          join iItem in db.InventoryItems on pd.fkItemId equals iItem.itemId
                          where rml.createdDate.Date == reportDate.Date
                          group pd by iItem.itemName into purchased
                          select new
                          {
                              itemName = purchased.Key.ToString(),
                              totalPurchased = purchased.Sum(x => x.quantity)
                          }).ToList<dynamic>();
            return result;
        }


        //public byte[] GeneratePDFReport(DateTime reportDate,
        //                                List<dynamic> totalSales,
        //                                List<dynamic> totalProduced,
        //                                List<dynamic> totalPurchased)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        Document document = new Document(PageSize.A4);
        //        PdfWriter.GetInstance(document, stream);
        //        document.Open();

        //        document.Add(new Paragraph($"Daily Report for {reportDate:yyyy-MM-dd}\n\n"));

        //        document.Add(new Paragraph("Total Sales Report:"));
        //        foreach (var item in totalSales)
        //        {
        //            document.Add(new Paragraph($"{item.ItemName} - {item.TotalSold} units"));
        //        }

        //        document.Add(new Paragraph("\nTotal Finished Goods Produced Report:"));
        //        foreach (var item in totalProduced)
        //        {
        //            document.Add(new Paragraph($"{item.ItemName} - {item.TotalProduced} units"));
        //        }

        //        document.Add(new Paragraph("\nTotal Items Purchased Report:"));
        //        foreach (var item in totalPurchased)
        //        {
        //            document.Add(new Paragraph($"{item.ItemName} - {item.TotalPurchased} units"));
        //        }

        //        document.Close();
        //        return stream.ToArray();
        //    }
        //}
    }
}
