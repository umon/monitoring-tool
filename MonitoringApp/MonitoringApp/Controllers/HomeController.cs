using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonitoringApp.Models;
using MonitoringApp.Repositories;

namespace MonitoringApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<TargetApplication> _targetAppRepo;

        public HomeController(ILogger<HomeController> logger, IRepository<TargetApplication> targetAppRepo)
        {
            _logger = logger;
            _targetAppRepo = targetAppRepo;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _targetAppRepo.List();

            return View();
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
