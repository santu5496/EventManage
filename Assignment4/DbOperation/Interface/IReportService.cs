using DbOperation.Models;

namespace DbOperation.Interface
{
    public interface IReportService
    {
        List<dynamic> GetTotalPurchased(DateTime reportDate);
        List<dynamic> GetTotalProduced(DateTime reportDate);
        List<dynamic> GetTotalSales(DateTime reportDate);
    }

}
