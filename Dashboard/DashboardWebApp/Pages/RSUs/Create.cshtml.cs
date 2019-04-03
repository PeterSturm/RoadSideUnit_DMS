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

namespace DashboardWebApp.Pages.RSUs
{
    public class CreateModel : PageModel
    {
        private readonly IRSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateModel(IRSUService rsuService, ApplicationDbContext applicationDbContext)
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
            Managers = _applicationDbContext.Managers
                .Select(m => new SelectListItem{
                    Value = m.Id.ToString(),
                    Text = $"{m.Name} {m.IP}/{m.Port}"})
                .ToList();
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

            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            await _rsuService.AddRSUAsync(RSU.Manager, user, RSU);

            return RedirectToPage("./Index");
        }
    }
}