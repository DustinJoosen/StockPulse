using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockPulse.Helpers;

namespace StockPulse.Controllers
{
    [Authorize(Roles = "admin")]
    public class SettingsController : Controller
    {
        private readonly INotyfService _notyf;
        
        public SettingsController(INotyfService notyf)
        {
            this._notyf = notyf;
        }

        public IActionResult Index()
        {
            var settings = BusinessSettingsLoader.Load();
            return View(settings);
        }

        [HttpPost]
        public IActionResult Index(BusinessSettings settings)
        {
            BusinessSettingsLoader.Save(settings);

            this._notyf.Information("Settings successfully updated");
            return RedirectToAction(nameof(Index));
        }
    }
}
