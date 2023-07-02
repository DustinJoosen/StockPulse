using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPulse.Data;
using StockPulse.Dtos;
using StockPulse.Models;
using System.Diagnostics;

namespace StockPulse.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Averages of other things

            var salaries = await this._context.Employees
                .Select(e => e.MonthlySalary).SumAsync();

            var user = await this._context.Employees
                .Include(u => u.Person)
                .SingleOrDefaultAsync(e => e.PersonEmail == User.Identity.Name);

            var sv = this._context.Products
                .Include(p => p.ProductStocks)
                .AsEnumerable()
                .Select(p => p.ProfitPerSell * p.TotalStock)
                .Sum();

            return View(new StatDto
            {
                AmountCustomers = await this._context.Customers.CountAsync(),
                AmountEmployees = await this._context.Employees.CountAsync(),
                AmountProducts = await this._context.Products.CountAsync(),
                AmountWarehouses = await this._context.Warehouses.CountAsync(),
                AmountOrders = await this._context.Orders.CountAsync(),
                MonthlyTotalSalary = salaries,
                YearlyTotalSalary = salaries * 12,
                TotalStocksValue = sv,
                User = user
            });
        }

    }
}