using StockPulse.Models;

namespace StockPulse.Dtos
{
    public class StatDto
    {
        public int AmountEmployees { get; set; }
        public int AmountCustomers { get; set; }
        public int AmountProducts { get; set; }
        public int AmountWarehouses { get; set; }
        public int AmountOrders { get; set; }
        public double MonthlyTotalSalary { get; set; }
        public double YearlyTotalSalary { get; set; }
        public double TotalStocksValue { get; set; }
        public Employee User { get; set; }
    }
}
