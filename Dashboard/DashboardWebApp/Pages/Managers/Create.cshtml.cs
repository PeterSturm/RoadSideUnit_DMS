using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.ViewModels.Manager;
using DashboardWebApp.ViewModels.RSU;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DashboardWebApp.Pages.Managers
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public ManagerEditModel Manager { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

           Manager manager = Manager.MapToManager(null);

            _applicationDbContext.Managers.Add(manager);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}