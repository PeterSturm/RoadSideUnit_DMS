using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebApp.Pages.ManagerLogs
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ManagerLogService _managerLogService;

        public IndexModel(ApplicationDbContext applicationDbContext, ManagerLogService managerLogService)
        {
            _applicationDbContext = applicationDbContext;
            _managerLogService = managerLogService;
        }

        public List<ManagerLog> ManagerLogs { get; set; }
        public Manager Manager { get; set; }

        public async Task<IActionResult> OnGetAsync(int? managerId)
        {
            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            ManagerLogs = new List<ManagerLog>();

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

                var managerlogs = await _managerLogService.GetAsync(managerUser);
                if (managerlogs != null)
                    ManagerLogs = managerlogs.ToList();
            }

            return Page();
        }
    }
}