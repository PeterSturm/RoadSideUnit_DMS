using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.ViewModels.TrapLog;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebApp.Pages.TrapLogs
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly TrapLogService _TrapLogService;
        private readonly RSUService _rsuService;

        public IndexModel(ApplicationDbContext applicationDbContext, TrapLogService TrapLogService, RSUService rsuService)
        {
            _applicationDbContext = applicationDbContext;
            _TrapLogService = TrapLogService;
            _rsuService = rsuService;
        }

        public List<TrapLogViewModel> TrapLogs { get; set; }
        public Manager Manager { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? managerId, int? rsuId)
        {
            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            TrapLogs = new List<TrapLogViewModel>();

            if (managerId.HasValue)
            {
                var manager = _applicationDbContext.Managers
                    .Include(m => m.Users)
                    .FirstOrDefault(m => m.Id == managerId);
                if (manager == null)
                    return NotFound($"There are no manager with id: {managerId}");
                if (manager.Users == null || manager.Users.Count == 0)
                    return NotFound($"There are no user for manager {managerId}");

                Manager = manager;

                var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;

                if (managerUser == null)
                    return NotFound($"There's no Manager User assigned to this User, with {manager.Name} Manager");

                IEnumerable<TrapLog> trapLogs;
                List<RSU> rsus = new List<RSU>();
                if (rsuId.HasValue)
                {
                    trapLogs = await _TrapLogService.GetAsync(managerUser, rsuId.Value);
                    if (trapLogs != null)
                    {
                        var rsu = await _rsuService.GetAsync(managerUser, rsuId.Value);
                        rsus.Add(rsu);
                    }
                }
                else
                {
                    trapLogs = await _TrapLogService.GetAsync(managerUser);
                    if (trapLogs != null)
                        rsus.AddRange(await _rsuService.GetAsync(managerUser));
                }
                
                if (trapLogs != null)
                {
                    TrapLogs = trapLogs.Select(tl => new TrapLogViewModel(rsus.FirstOrDefault(r => r.Id == tl.SourceRsuId),tl)).ToList();
                }
            }

            return Page();
        }
    }
}