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
using Microsoft.EntityFrameworkCore;

namespace DashboardWebApp.Pages.Managers
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserService _userService;

        public EditModel(ApplicationDbContext applicationDbContext, UserService userService)
        {
            _applicationDbContext = applicationDbContext;
            _userService = userService;
        }

        [BindProperty]
        public ManagerEditModel ManagerEditM { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var manager = await _applicationDbContext.Managers
                .Include(m => m.Users)
                .SingleOrDefaultAsync(m => m.Id == id.Value);
            if (manager == null)
                return NotFound();

            ManagerEditM = ManagerEditModel.Parse(manager);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var manager = _applicationDbContext.Managers
                .Include(m => m.Users)
                .SingleOrDefault(m => m.Id == ManagerEditM.Id);
            if (manager == null)
                return NotFound();

            foreach (var managerUser in ManagerEditM.Users)
            {

            }

            manager = ManagerEditM.MapToManager(manager);

            _applicationDbContext.Managers.Update(manager);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public void ChangeManagerUserStatus(int id, Status newStatus)
        {
            var managerUserEM = ManagerEditM.ManagerUserEditModels.SingleOrDefault(mu => mu.Id == id);
            if (managerUserEM != null)
                managerUserEM.Status = newStatus;
        }

        public void AddManagerUser()
        {
            ManagerUserEditModel newManagerUserEditModel = new ManagerUserEditModel();
            newManagerUserEditModel.Status = Status.CREATED;
            newManagerUserEditModel.ManagerId = ManagerEditM.Id;
            newManagerUserEditModel.Manager = _applicationDbContext.Managers
                .Include(m => m.Users)
                .SingleOrDefault(m => m.Id == ManagerEditM.Id);

            ManagerEditM.ManagerUserEditModels.Add(newManagerUserEditModel);

            Page();
        }
    }
}