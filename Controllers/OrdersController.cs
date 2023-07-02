using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPulse.Data;
using StockPulse.Models;

namespace StockPulse.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public OrdersController(ApplicationDbContext context, INotyfService notyf)
        {
            this._context = context;
            this._notyf = notyf;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await this._context.Orders
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .Include(o => o.Customer)
                .ToListAsync();

            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderNum == id);

            if (order == null)
            {
                return NotFound();
            }

            var products = await this._context.Products
                .OrderBy(p => p.Name)
                .ToListAsync();

            foreach (var orderline in order.OrderLines)
            {
                products.Remove(orderline.Product);
            }

            ViewData["Products"] = new SelectList(products, "ProductNum", "Name");
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderLine(int ordernum, int product)
        {
            if (ordernum == 0 || product == 0)
            {
                return NotFound();
            }

            int first_warehouse_id = (await this._context.Warehouses.FirstOrDefaultAsync()).Id;

            this._context.OrderLines.Add(new OrderLine
            {
                OrderNum = ordernum,
                ProductNum = product,
                WarehouseId = first_warehouse_id
            });
            await this._context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new
            {
                Id = ordernum
            });
        }

        public async Task<IActionResult> DeleteOrderLine(int ordernum, int productnum)
        {
            if (ordernum == 0 || productnum == 0)
            {
                return NotFound();                
            }

            var orderline = await this._context.OrderLines
                .SingleOrDefaultAsync(ol => ol.OrderNum == ordernum && ol.ProductNum == productnum);

            if (orderline == null)
            {
                return NotFound();
            }

            this._context.OrderLines.Remove(orderline);
            await this._context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new
            {
                Id = ordernum
            });
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerEmail"] = new SelectList(_context.Customers, "PersonEmail", "PersonEmail");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerEmail,DiscountPrice,DeliveryNotes")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();

                this._notyf.Success($"Successfully created order");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerEmail"] = new SelectList(_context.Customers, "PersonEmail", "PersonEmail", order.CustomerEmail);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["CustomerEmail"] = new SelectList(_context.Customers, "PersonEmail", "PersonEmail", order.CustomerEmail);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderNum,CustomerEmail,DiscountPrice,DeliveryNotes")] Order order)
        {
            if (id != order.OrderNum)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) { throw; }


                this._notyf.Success("Successfully updated order");
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerEmail"] = new SelectList(_context.Customers, "PersonEmail", "PersonEmail", order.CustomerEmail);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderNum == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();

            this._notyf.Success("Successfully deleted order");
            return RedirectToAction(nameof(Index));
        }

    }
}
