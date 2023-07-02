using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPulse.Data;
using StockPulse.Models;
using Microsoft.AspNetCore.Authorization;


namespace StockPulse.Controllers
{
    [Authorize]
    public class StocksController : Controller
    {

        private ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stocks = await this._context.ProductStocks
                .Include(p => p.Product)
                .Include(p => p.Warehouse)
                .ToListAsync();

            var products = await this._context.Products
                .Include(p => p.ProductStocks)
                .ToListAsync();

            var warehouses = await this._context.Warehouses
                .Include(w => w.ProductStocks)
                .ToListAsync();

            return View(new StockPageDto
            {
                Stocks = stocks,
                Products = products,
                Warehouses = warehouses
            });
        }

        [HttpPost]
        public async Task<IActionResult> ModifyQuantity(ProductStock productStock, string returnUrl = null)
        {
            if (productStock == null)
            {
                return NotFound();
            }

            var stock = await this._context.ProductStocks
                .Where(ps => ps.ProductNum == productStock.ProductNum)
                .Where(ps => ps.WareHouseId == productStock.WareHouseId)
                .FirstOrDefaultAsync();

            if (stock == null)
            {
                return NotFound();
            }

            // Make 0 the minimal value
            stock.Quantity = (productStock.Quantity > 0) ? productStock.Quantity : 0;

            this._context.ProductStocks.Update(stock);
            await this._context.SaveChangesAsync();

            if (returnUrl == null)
            {
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                return Redirect(returnUrl);
            }

        }
    }
}
