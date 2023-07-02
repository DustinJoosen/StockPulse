using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPulse.Data;
using StockPulse.Models;
using Microsoft.AspNetCore.Authorization;

namespace StockPulse.Controllers
{
    [Authorize]
    public class WarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public WarehousesController(ApplicationDbContext context, INotyfService notyf)
        {
            this._context = context;
            this._notyf = notyf;
        }

        // GET: Warehouses
        public async Task<IActionResult> Index()
        {
            var warehouses = await this._context.Warehouses
                .Include(w => w.Manager)
                .ToListAsync();

            return View(warehouses);
        }

        // GET: Warehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .Include(w => w.Manager)
                .Include(w => w.ProductStocks)
                    .ThenInclude(ps => ps.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // GET: Warehouses/Create
        public IActionResult Create()
        {
            ViewData["ManagerEmail"] = new SelectList(_context.Employees, "PersonEmail", "PersonEmail");
            return View();
        }

        // POST: Warehouses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Street,ZipCode,City,ManagerEmail")] Warehouse warehouse)
        {

            // Remove from validation
            ModelState.Remove(nameof(warehouse.Manager));
            ModelState.Remove(nameof(warehouse.ProductStocks));

            if (ModelState.IsValid)
            {
                // Add the warehouse
                this._context.Add(warehouse);
                await this._context.SaveChangesAsync();
                
                // Set all products to 0 for the warehouse
                var products = await this._context.Products.ToListAsync();
                foreach (var product in products)
                {
                    this._context.ProductStocks.Add(new ProductStock
                    {
                        ProductNum = product.ProductNum,
                        WareHouseId = warehouse.Id,
                        Quantity = 0
                    });
                }

                await this._context.SaveChangesAsync();

                this._notyf.Success("Successfully created warehouse");
                return RedirectToAction(nameof(Index));
            }

            this._notyf.Error("Could not create warehouse");

            ViewData["ManagerEmail"] = new SelectList(_context.Employees, "PersonEmail", "PersonEmail", warehouse.ManagerEmail);
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            ViewData["ManagerEmail"] = new SelectList(_context.Employees, "PersonEmail", "PersonEmail", warehouse.ManagerEmail);
            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Street,ZipCode,City,ManagerEmail")] Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return NotFound();
            }

            // Remove from validation
            ModelState.Remove(nameof(warehouse.Manager));
            ModelState.Remove(nameof(warehouse.ProductStocks));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();

                    this._notyf.Success("Successfully updated warehouse");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }

            this._notyf.Error("Could not update warehouse");

            ViewData["ManagerEmail"] = new SelectList(_context.Employees, "PersonEmail", "PersonEmail", warehouse.ManagerEmail);
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .Include(w => w.Manager)
                .Include(w => w.ProductStocks)
                    .ThenInclude(ps => ps.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
            }
            
            await _context.SaveChangesAsync();

            this._notyf.Success("Successfully deleted warehouse");
            return RedirectToAction(nameof(Index));
        }
    }
}
