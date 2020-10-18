using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringApp.Models;
using MonitoringApp.Repositories;
using MonitoringApp.Services;
using MonitoringApp.ViewModels;

namespace MonitoringApp.Controllers
{
    [Authorize]
    public class TargetApplicationController : Controller
    {
        private readonly IRepository<TargetApplication> _targetAppRepo;
        private readonly IHealthCheckService _healthCheckService;


        public TargetApplicationController(IRepository<TargetApplication> targetAppRepo, IHealthCheckService healthCheckService)
        {
            _targetAppRepo = targetAppRepo;
            _healthCheckService = healthCheckService;
        }

        public async Task<ActionResult> Index()
        {
            var userGuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var targerApplicationList = await _targetAppRepo.List(x => x.UserGuid == userGuid);

            var model = targerApplicationList.Adapt<IEnumerable<TargetApplicationModelForDetails>>();

            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var userGuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var targetApplication = await _targetAppRepo.Get(x => x.UserGuid == userGuid && x.Id == id);

            if (targetApplication == null)
                return NotFound();

            var viewModel = targetApplication.Adapt<TargetApplicationModelForDetails>();

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View(new TargetApplicationModelForCreate());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TargetApplicationModelForCreate viewModel)
        {
            var userGuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                var applicationNameExists = await _targetAppRepo
                    .Exists(x => x.UserGuid == userGuid && EF.Functions.Like(x.Name, viewModel.Name.Trim()));

                if (applicationNameExists)
                {
                    ModelState.AddModelError("Name", "This name is used in another application");
                    return View(viewModel);
                }

                var targetApplication = viewModel.Adapt<TargetApplication>();
                targetApplication.UserGuid = userGuid;
                targetApplication.Status = Enums.MonitorStatus.Stopped;

                await _targetAppRepo.Add(targetApplication);

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);

        }

        public async Task<ActionResult> Edit(int id)
        {
            var userGuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var targetApplication = await _targetAppRepo.Get(x => x.UserGuid == userGuid && x.Id == id);

            if (targetApplication == null)
                return NotFound();

            var viewModel = targetApplication.Adapt<TargetApplicationModelForEdit>();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TargetApplicationModelForEdit viewModel)
        {
            if (ModelState.IsValid)
            {
                var userGuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var targetApplication = await _targetAppRepo.Get(x => x.UserGuid == userGuid && x.Id == id);

                if (targetApplication == null)
                    return NotFound();

                var applicationNameExists = await _targetAppRepo
                    .Exists(x => x.UserGuid == userGuid && x.Id != targetApplication.Id
                    && EF.Functions.Like(x.Name, viewModel.Name.Trim()));

                if (applicationNameExists)
                {
                    ModelState.AddModelError("Name", "This name is used in another application");
                    return View(viewModel);
                }

                targetApplication.Interval = viewModel.Interval;
                targetApplication.Name = viewModel.Name;
                targetApplication.Url = viewModel.Url;
                targetApplication.NotificationMail = viewModel.NotificationMail;

                await _targetAppRepo.Update(targetApplication);

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);

        }

        public async Task<ActionResult> Delete(int id)
        {
            var userGuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var targetApplication = await _targetAppRepo.Get(x => x.UserGuid == userGuid && x.Id == id);

            if (targetApplication != null)
                await _targetAppRepo.Delete(targetApplication);

            _healthCheckService.RemoveJob(id.ToString());

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> ChangeStatus(int id)
        {
            var userGuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var targetApplication = await _targetAppRepo.Get(x => x.UserGuid == userGuid && x.Id == id);

            if (targetApplication == null)
            {
                return NotFound();
            }

            _healthCheckService.RemoveJob(targetApplication.Id.ToString());

            if (targetApplication.Status == Enums.MonitorStatus.Monitoring)
            {
                targetApplication.Status = Enums.MonitorStatus.Stopped;
            }
            else
            {
                _healthCheckService.AddJob(targetApplication);
                targetApplication.Status = Enums.MonitorStatus.Monitoring;
            }

            await _targetAppRepo.Update(targetApplication);

            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
