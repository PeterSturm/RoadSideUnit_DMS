using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.ViewModels.RSU;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebApp.Pages.RSUs
{
    public class CreateModel : PageModel
    {
        private readonly RSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateModel(RSUService rsuService, ApplicationDbContext applicationDbContext)
        {
            _rsuService = rsuService;
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public RSUAddModel RSU { get; set; }

        [BindProperty]
        public List<SelectListItem> Managers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Managers = await _applicationDbContext.Managers
                .Select(m => new SelectListItem{
                    Value = m.Id.ToString(),
                    Text = $"{m.Name} {m.IP}/{m.Port}"})
                .ToListAsync();
            Managers.Insert(0, new SelectListItem {  Value = "-1", Text = "Select" });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (RSU.ManagerId == -1)
                return Page();

            RSU.Manager = _applicationDbContext.Managers.Find(RSU.ManagerId);

            if (!ModelState.IsValid)
                return Page();

            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == RSU.ManagerId)?.ManagerUser;
            if (managerUser == null)
                return NotFound($"There's no Manager User assigned to this User, with {RSU.Manager.Name} Manager");

            await _rsuService.AddAsync(managerUser, RSU);

            return RedirectToPage("./Index");
        }
    }
}