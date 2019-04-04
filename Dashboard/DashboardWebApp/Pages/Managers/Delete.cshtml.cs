using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DashboardWebApp.Pages.Managers
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public Manager Manager { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();


            Manager = _applicationDbContext.Managers.FirstOrDefault(m => m.Id == id.Value);
            if (Manager == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var manager = _applicationDbContext.Managers.FirstOrDefault(m => m.Id == id.Value);
            if (manager == null)
                return NotFound();

            _applicationDbContext.Managers.Remove(manager);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}