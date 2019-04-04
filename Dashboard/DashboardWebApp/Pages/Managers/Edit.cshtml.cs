using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.ViewModels.Manager;
using DashboardWebApp.ViewModels.RSU;
using DashboardWebApp.WebApiClients;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DashboardWebApp.Pages.Managers
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EditModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public ManagerEditModel ManagerEditM { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var manager = _applicationDbContext.Managers.SingleOrDefault(m => m.Id == id.Value);
            if (manager == null)
                return NotFound();

            ManagerEditM = ManagerEditModel.Parse(manager);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            /*_applicationDbContext.Managers.Update(new Manager {
                Id = ManagerEditM.Id,
                Name = ManagerEditM.Name,
                IP = ManagerEditM.IP,
                Port = ManagerEditM.Port});*/

            var manager = _applicationDbContext.Managers.SingleOrDefault(m => m.Id == ManagerEditM.Id);
            if (manager == null)
                return NotFound();

            manager = ManagerEditM.MapToManager(manager);

            _applicationDbContext.Managers.Update(manager);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}