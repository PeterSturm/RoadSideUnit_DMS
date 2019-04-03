using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DashboardWebApp.Pages.RSUs
{
    public class DeleteModel : PageModel
    {
        private readonly IRSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteModel(IRSUService rsuService, ApplicationDbContext applicationDbContext)
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

            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            if (!managerId.HasValue)
                return NotFound();

            var manager = _applicationDbContext.Managers.FirstOrDefault(m => m.Id == managerId.Value);
            if (manager == null)
                return NotFound();

            RSU = await _rsuService.GetAsync(manager, user, id.Value);

            if (RSU == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int? managerId)
        {
            if (!id.HasValue)
                return NotFound();

            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            if (!managerId.HasValue)
                return NotFound();

            var manager = _applicationDbContext.Managers.FirstOrDefault(m => m.Id == managerId.Value);
            if (manager == null)
                return NotFound();

            RSU = await _rsuService.GetAsync(manager, user, id.Value);

            if (RSU == null)
                return NotFound();
            else
                await _rsuService.DeleteRSUAsync(manager, user, id.Value);


            return RedirectToPage("./Index");
        }
    }
}