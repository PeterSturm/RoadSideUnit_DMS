using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DashboardWebApp.Models;
using Microsoft.AspNetCore.Identity;
using DashboardWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebApp.Pages.RSUs
{
    public class IndexModel : PageModel
    {
        private readonly RSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexModel(RSUService rsuService, ApplicationDbContext applicationDbContext)
        {
            _rsuService = rsuService;
            _applicationDbContext = applicationDbContext;
        }

        public List<RSU> RSUs { get; private set; }

        public async Task OnGetAsync(int? managerId)
        {
            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            RSUs = new List<RSU>();

            if (managerId.HasValue)
            {
                var manager = _applicationDbContext.Managers
                    .Include(m => m.Users)
                    .FirstOrDefault(m => m.Id == managerId);
                if (manager == null)
                {
                    NotFound($"No manager with id: {managerId}");
                }

                var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;

                if (managerUser == null)
                    NotFound($"There's no Manager User assigned to this User, with {manager.Name} Manager");

                var rsus = await _rsuService.GetAsync(managerUser);
                if(rsus != null)
                    RSUs = rsus.ToList();
            }
            else
            {
                var managers = _applicationDbContext.Managers
                    .Include(m => m.Users)
                    .ToList();
                if (managers == null)
                    NotFound("There are no managers");

                foreach (var manager in managers)
                {
                    var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;
                    if (managerUser != null)
                    {
                        var rsus = await _rsuService.GetAsync(managerUser);
                        if (rsus != null)
                            RSUs.AddRange(rsus);
                    }
                }
            }
        }
    }
}