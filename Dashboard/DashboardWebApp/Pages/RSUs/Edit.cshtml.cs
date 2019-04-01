using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.ViewModels.RSU;
using DashboardWebApp.WebApiClients;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DashboardWebApp.Pages.RSUs
{
    public class EditModel : PageModel
    {
        private readonly IRSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public EditModel(IRSUService rsuService, ApplicationDbContext applicationDbContext)
        {
            _rsuService = rsuService;
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public RSUEditModel RSUEditM { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id, int? managerId)
        {
            if (id == null)
                return NotFound();

            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            if (managerId == null)
                return NotFound();

            var manager = _applicationDbContext.Managers.FirstOrDefault(m => m.Id == managerId.Value);
            if (manager == null)
                return NotFound();

            RSUEditM = RSUEditModel.Parse( await _rsuService.GetAsync(manager, user, id.Value), manager);

            if (RSUEditM == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            var prevManager = _applicationDbContext.Managers.FirstOrDefault(m => m.IP.ToString() == RSUEditM.prevMIP && m.Port == RSUEditM.prevMPort);
            if (prevManager == null)
            { 
                // TODO 
            }

            var manager = _applicationDbContext.Managers.FirstOrDefault(m => m.IP.ToString() == RSUEditM.prevMIP && m.Port == RSUEditM.prevMPort);
            if (manager == null)
            {
                // TODO 
            }

            if (prevManager.IP.ToString() != RSUEditM.ManagerIP || prevManager.Port != RSUEditM.ManagerPort)
            {
                await _rsuService.DeleteRSUAsync(prevManager, user, RSUEditM.Id);
                await _rsuService.AddRSUAsync(manager, user, RSUEditM.ConvertToRSU(manager));
            }
            else
            {
                await _rsuService.UpdateRSUAsync(manager, user, RSUEditM.ConvertToRSU(manager));
            }

            return RedirectToPage("./Index");
        }
    }
}