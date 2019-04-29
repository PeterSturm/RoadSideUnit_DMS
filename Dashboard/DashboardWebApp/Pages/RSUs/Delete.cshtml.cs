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

namespace DashboardWebApp.Pages.RSUs
{
    public class DeleteModel : PageModel
    {
        private readonly RSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteModel(RSUService rsuService, ApplicationDbContext applicationDbContext)
        {
            _rsuService = rsuService;
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public RSU RSU { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? managerId)
        {
            if (!id.HasValue)
                return NotFound();

            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            if (!managerId.HasValue)
                return NotFound();

            var manager = _applicationDbContext.Managers
                .Include(m => m.Users)
                .FirstOrDefault(m => m.Id == managerId.Value);
            if (manager == null)
                return NotFound();

            var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == managerId)?.ManagerUser;
            if (managerUser == null)
                NotFound($"There's no Manager User assigned to this User, with {RSU.Manager.Name} Manager");

            RSU = await _rsuService.GetAsync(managerUser, id.Value);

            if (RSU == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int? managerId)
        {
            if (!id.HasValue)
                return NotFound();

            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            if (!managerId.HasValue)
                return NotFound();

            var manager = _applicationDbContext.Managers
                .Include(m => m.Users)
                .FirstOrDefault(m => m.Id == managerId.Value);
            if (manager == null)
                return NotFound();

            var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == managerId)?.ManagerUser;
            if (managerUser == null)
                return NotFound($"There's no Manager User assigned to this User, with {RSU.Manager.Name} Manager");

            RSU = await _rsuService.GetAsync(managerUser, id.Value);

            if (RSU == null)
                return NotFound();
            else
                await _rsuService.DeleteAsync(managerUser, id.Value);


            return RedirectToPage("./Index");
        }
    }
}