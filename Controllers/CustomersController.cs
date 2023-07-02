using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPulse.Data;
using StockPulse.Dtos;
using StockPulse.Models;
using Microsoft.AspNetCore.Authorization;

namespace StockPulse.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public CustomersController(ApplicationDbContext context, INotyfService notyf)
        {
            this._context = context;
            this._notyf = notyf;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await this._context.Customers
                .Include(c => c.Person)
                .ToListAsync();

            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Person)
                .FirstOrDefaultAsync(m => m.PersonEmail == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                this._context.Persons.Add(customerDto.ExtractPerson());
                this._context.Customers.Add(customerDto.ExtractCustomer());

                await _context.SaveChangesAsync();

                this._notyf.Success($"Successfully created customer");
                return RedirectToAction(nameof(Index));
            }

            return View(customerDto);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.PersonEmail == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(CustomerDto.Combine(customer, customer.Person));
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CustomerDto customerDto)
        {
            if (id != customerDto.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._context.Persons.Update(customerDto.ExtractPerson());
                    this._context.Customers.Update(customerDto.ExtractCustomer());

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) { throw; }

                this._notyf.Success("Successfully updated customer");
                return RedirectToAction(nameof(Index));
            }

            return View(customerDto);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Person)
                .FirstOrDefaultAsync(m => m.PersonEmail == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var customer = await _context.Customers
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.PersonEmail == id);

            if (customer == null || customer.Person == null)
            {
                return NotFound();
            }

            this._context.Customers.Remove(customer);
            this._context.Persons.Remove(customer.Person);

            await _context.SaveChangesAsync();

            this._notyf.Success("Successfully deleted customer");
            return RedirectToAction(nameof(Index));
        }
    }
}
