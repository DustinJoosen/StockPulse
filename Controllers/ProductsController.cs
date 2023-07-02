using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPulse.Data;
using StockPulse.Helpers;
using StockPulse.Interfaces;
using StockPulse.Models;
using Microsoft.AspNetCore.Authorization;


namespace StockPulse.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly INotyfService _notyf;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env, INotyfService notyf)
        {
            this._context = context;
            this._env = env;
            this._notyf = notyf;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await this._context.Products
                .Include(p => p.ProductStocks)
                .ToListAsync();

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductStocks)
                    .ThenInclude(ps => ps.Warehouse)
                .FirstOrDefaultAsync(p => p.ProductNum == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ModelState.Remove(nameof(product.ProductStocks));

            if (ModelState.IsValid)
            {
                // Save image.
                product.ImagePath = ImageStorageManager.UploadImage(product, this._env);

                // Save product.
                this._context.Add(product);
                await _context.SaveChangesAsync();

                // Add product to all warehouses.
                var warehouses = await this._context.Warehouses.ToListAsync();
                foreach (var warehouse in warehouses)
                {
                    this._context.ProductStocks.Add(new ProductStock
                    {
                        ProductNum = product.ProductNum,
                        WareHouseId = warehouse.Id,
                        Quantity = 0
                    });
                    await this._context.SaveChangesAsync();
                }

                this._notyf.Success($"Successfully created product [{product.Name}]");
                return RedirectToAction(nameof(Index));
            }

            this._notyf.Error("Could not create product");
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductNum)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(product.ProductStocks));

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.FormFile != null)
                    {
                        // Remove the exisiting image
                        ImageStorageManager.RemoveImage(product, this._env);

                        // Make a new image
                        product.ImagePath = ImageStorageManager.UploadImage(product, this._env);
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    this._notyf.Success($"Successfully updated product [{product.Name}]");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException) { }
            }

            this._notyf.Error("Could not update product");
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductStocks)
                    .ThenInclude(ps => ps.Warehouse)
                .FirstOrDefaultAsync(m => m.ProductNum == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Delete));
            }

            ImageStorageManager.RemoveImage(product, this._env);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            this._notyf.Success("Successfully deleted product");
            return RedirectToAction(nameof(Index));
        }

    }
}
