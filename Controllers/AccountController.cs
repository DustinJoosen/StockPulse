using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPulse.Data;
using StockPulse.Dtos;
using StockPulse.Helpers;
using StockPulse.Models;
using System.Security.Claims;

namespace StockPulse.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _env;
        public AccountController(ApplicationDbContext context, INotyfService notyf, IWebHostEnvironment env)
        {
            this._context = context;
            this._notyf = notyf;
            this._env = env;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthDto auth, bool showNotyfMessage=true)
        {
            // Guard clauses
            if (!ModelState.IsValid)
            {
                return View(auth);
            }

            if (!await this.IsLoginValid(auth))
            {
                ModelState.AddModelError(String.Empty, "Invalid login attempt");
                return View(auth);
            }

            // Get the user. At this point, the user is valid and can be claimed.
            var user = await this._context.Employees
                .Include(e => e.Roles)
                .Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.PersonEmail == auth.Email);

            // Add the name claim
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.PersonEmail.ToString())
            };

            // Add the role claims
            claims.AddRange(user.Roles.Select(role =>
                new Claim(ClaimTypes.Role, role.Name.ToString())));

            // Sign in with the httpcontext
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.UtcNow.AddDays(28),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            // Notify the success, and redirect to the dashboard
            if (showNotyfMessage)
            {
                _notyf.Information($"Successfully signed in. Welcome {user.Person.FullName}");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            _notyf.Information("Logged out");
            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid || registerDto.SaltedPassword != registerDto.SaltedPasswordConfirmation)
            {
                return View(registerDto);
            }
            
            if (this._context.IsEmailUsed(registerDto.Email))
            {
                ModelState.AddModelError(nameof(registerDto.Email), "Email is already in use");
                return View(registerDto);
            }

            // Extract the person and employee info
            Person person = new Person
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                Particle = registerDto.Particle,
                Lastname = registerDto.Lastname,
                Pronouns = registerDto.Pronouns,
                PhoneNumber = registerDto.PhoneNumber
            };

            Employee employee = new Employee
            {
                PersonEmail = person.Email,
                SaltedPassword = registerDto.SaltedPassword,
                EmployeeSince = DateTime.UtcNow,
                MonthlySalary = 2500f,
                FormFile = registerDto.FormFile
            };

            // Try to add the profile picture image
            employee.SetImagePath(ImageStorageManager.UploadImage(employee, this._env));

            // Add the person, roles and employee to the database
            this._context.Persons.Add(person);
            this._context.Employees.Add(employee);
            this._context.EmployeeRoles.Add(new EmployeeRole
            {
                EmployeeEmail = employee.PersonEmail,
                Name = "employee"
            });

            // save changes and throw a notyf.
            await this._context.SaveChangesAsync();
            this._notyf.Information("Registration successful! Welcome " + person.FullName + "!");

            // Let it log in.
            return await this.Login(new AuthDto
            {
                Email = registerDto.Email,
                Password = registerDto.SaltedPassword
            }, false);
        }

        private async Task<bool> IsLoginValid(AuthDto auth)
        {
            // If no auth, that's a clear no
            if (auth == null)
            {
                return false;
            }

            // Get the user
            var user = await this._context.Employees
                .FirstOrDefaultAsync(e => e.PersonEmail == auth.Email);

            // If user doesn't exist, that's a clear no
            if (user == null)
            {
                return false;
            }

            // TODO: Implement hashing
            string hashed_pwd = (auth.Password);

            return user.SaltedPassword.Equals(hashed_pwd);
        }
    }
}
