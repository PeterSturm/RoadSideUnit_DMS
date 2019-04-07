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
        private readonly RSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public EditModel(RSUService rsuService, ApplicationDbContext applicationDbContext)
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

            var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == managerId)?.ManagerUser;
            if (managerUser == null)
                NotFound($"There's no Manager User assigned to this User, with {manager.Name} Manager");

            RSUEditM = RSUEditModel.Parse( await _rsuService.GetAsync(managerUser, id.Value), manager);

            if (RSUEditM == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

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
                var managerUserprev = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == prevManager.Id)?.ManagerUser;
                if (managerUserprev == null)
                    NotFound($"There's no Manager User assigned to this User, with {prevManager.Id} Manager");

                var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;
                if (managerUser == null)
                    NotFound($"There's no Manager User assigned to this User, with {manager.Name} Manager");

                await _rsuService.DeleteAsync(managerUserprev, RSUEditM.Id);
                await _rsuService.AddAsync(managerUser, RSUEditM.MapToRSUWithManager(manager));
            }
            else
            {
                var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;
                if (managerUser == null)
                    NotFound($"There's no Manager User assigned to this User, with {manager.Name} Manager");

                await _rsuService.UpdateAsync(managerUser, RSUEditM.MapToRSUWithManager(manager));
            }

            return RedirectToPage("./Index");
        }
    }
}