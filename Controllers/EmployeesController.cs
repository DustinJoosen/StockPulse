using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPulse.Data;
using StockPulse.Dtos;
using StockPulse.Helpers;
using StockPulse.Models;
using Microsoft.AspNetCore.Authorization;

namespace StockPulse.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _env;

        public EmployeesController(ApplicationDbContext context, INotyfService notyf, IWebHostEnvironment env)
        {
            this._context = context;
            this._notyf = notyf;
            this._env = env;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await this._context.Employees
                .Include(c => c.Person)
                .Include(e => e.Roles)
                .ToListAsync();

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Either you're an admin, or you're the employee
            if (!(User.IsInRole("admin") || User.Identity.Name.Equals(id)))
            {
                return Unauthorized();
            }

            var employee = await _context.Employees
                .Include(c => c.Person)
                .Include(e => e.Warehouse)
                .FirstOrDefaultAsync(m => m.PersonEmail == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            // Either you're an admin, or you're the employee
            if (!User.IsInRole("admin"))
            {
                return Unauthorized();
            }

            BusinessSettings settings = BusinessSettingsLoader.Load();

            return View(new EmployeeDto
            {
                MonthlySalary = settings.DefaultEmployeeSalary
            });
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDto employeeDto)
        {
            // Either you're an admin, or you're the employee
            if (!User.IsInRole("admin"))
            {
                return Unauthorized();
            }

            // Email should be unique.
            if (this._context.IsEmailUsed(employeeDto.Email))
            {
                ModelState.AddModelError(nameof(employeeDto.Email), "Email is already in use");
                return View(employeeDto);
            }

            // Set the password according to the appsettings
            BusinessSettings settings = BusinessSettingsLoader.Load();
            employeeDto.SaltedPassword = settings.DefaultEmployeePassword;

            ModelState.Remove(nameof(employeeDto.SaltedPassword));

            if (ModelState.IsValid)
            {
                var employee = employeeDto.ExtractEmployee();

                // Insert image
                employee.SetImagePath(ImageStorageManager.UploadImage(employee, this._env));

                // Add to the db
                this._context.Persons.Add(employeeDto.ExtractPerson());
                this._context.Employees.Add(employee);
                this._context.EmployeeRoles.Add(new EmployeeRole
                {
                    EmployeeEmail = employee.PersonEmail,
                    Name = "employee"
                });

                // If destined to be an admin, add that
                if (employeeDto.IsAdmin)
                {
                    this._context.EmployeeRoles.Add(new EmployeeRole
                    {
                        EmployeeEmail = employee.PersonEmail,
                        Name = "admin"
                    });
                }

                await _context.SaveChangesAsync();

                this._notyf.Success($"Successfully created employee");
                return RedirectToAction(nameof(Index));
            }

            return View(employeeDto);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Either you're an admin, or you're the employee
            if (!(User.IsInRole("admin") || User.Identity.Name.Equals(id)))
            {
                return Unauthorized();
            }

            var employee = await _context.Employees
                .Include(c => c.Person)
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(c => c.PersonEmail == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(EmployeeDto.Combine(employee, employee.Person));
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EmployeeDto employeeDto)
        {
            if (id != employeeDto.Email)
            {
                return NotFound();
            }

            // Either you're an admin, or you're the employee
            if (!(User.IsInRole("admin") || User.Identity.Name.Equals(id)))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Replace image
                    Employee employee = employeeDto.ExtractEmployee();

                    if (employeeDto.FormFile != null)
                    {
                        ImageStorageManager.RemoveImage(employee, this._env);
                        employee.SetImagePath(ImageStorageManager.UploadImage(employee, this._env));
                    }

                    // Look at the admin lore
                    await this.ApplyAdminRoleChanges(employeeDto.Email, employeeDto.IsAdmin);

                    // Add to db
                    this._context.Persons.Update(employeeDto.ExtractPerson());
                    this._context.Employees.Update(employee);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) { throw; }

                this._notyf.Success("Successfully updated employee");
                return RedirectToAction(nameof(Index));
            }

            return View(employeeDto);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Either you're an admin, or you're the employee
            if (!(User.IsInRole("admin") || User.Identity.Name.Equals(id)))
            {
                return Unauthorized();
            }

            var employee = await _context.Employees
                .Include(c => c.Person)
                .FirstOrDefaultAsync(m => m.PersonEmail == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Either you're an admin, or you're the employee
            if (!(User.IsInRole("admin") || User.Identity.Name.Equals(id)))
            {
                return Unauthorized();
            }

            var employee = await _context.Employees
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.PersonEmail == id);

            if (employee == null || employee.Person == null)
            {
                return NotFound();
            }

            this._context.Employees.Remove(employee);
            this._context.Persons.Remove(employee.Person);

            await _context.SaveChangesAsync();

            this._notyf.Success("Successfully deleted employee");
            return RedirectToAction(nameof(Index));
        }


        private async Task ApplyAdminRoleChanges(string employee_email, bool should_be_admin)
        {
            if (!User.IsInRole("admin"))
            {
                return;
            }

            // Check if it already is an admin
            bool is_already_an_admin = await this._context.EmployeeRoles
                .AnyAsync(e => e.EmployeeEmail == employee_email && e.Name == "admin");

            // The admin role, to either add or delete to the db.
            EmployeeRole admin_role_for_this_employee = new EmployeeRole
            {
                EmployeeEmail = employee_email,
                Name = "admin"
            };

            // If it should be, but is not, an admin.
            if (should_be_admin && !is_already_an_admin)
            {
                this._context.EmployeeRoles.Add(admin_role_for_this_employee);
            }

            // If it should NOT be, but IS, and admin.
            if (!should_be_admin && is_already_an_admin)
            {
                this._context.EmployeeRoles.Remove(admin_role_for_this_employee);
            }

            await this._context.SaveChangesAsync();

        }
    }
}
