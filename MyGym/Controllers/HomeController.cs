using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyGym.Models;
using System.Diagnostics;

namespace MyGym.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalyticsService _analyticsService;

        public HomeController(ILogger<HomeController> logger ,IAnalyticsService analyticsService)
        {
            _logger = logger;
           _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var data =await _analyticsService.GetDataAsync(ct);
            if (data.success)
            {
                return View(data.Value);
            }
            return View(data.Value);
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
