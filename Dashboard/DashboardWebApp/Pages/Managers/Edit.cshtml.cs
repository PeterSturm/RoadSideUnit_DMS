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
using Common.DTO;
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
        public ManagerEditModel ManagerEditModel { get; set; }
        [BindProperty]
        public List<ManagerUserEditModel> ManagerUserEditModels { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var manager = await _applicationDbContext.Managers
                .Include(m => m.Users)
                .SingleOrDefaultAsync(m => m.Id == id.Value);
            if (manager == null)
                return NotFound();

            ManagerEditModel = ManagerEditModel.Parse(manager);
            ManagerUserEditModels = manager.Users.Select(mu => new ManagerUserEditModel(mu)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostManagerEditAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var manager = _applicationDbContext.Managers
                .Include(m => m.Users)
                .SingleOrDefault(m => m.Id == ManagerEditModel.Id);
            if (manager == null)
                return NotFound();

            manager = ManagerEditModel.MapToManager(manager);

            _applicationDbContext.Managers.Update(manager);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public void ChangeManagerUserStatus(int id, Status newStatus)
        {
            var managerUserEM = ManagerUserEditModels.SingleOrDefault(mu => mu.Id == id);
            if (managerUserEM != null)
                managerUserEM.Status = newStatus;
        }

        public async Task<IActionResult> OnPostAddManagerUser()
        {
            ManagerUserEditModel newManagerUserEditModel = new ManagerUserEditModel();
            newManagerUserEditModel.Status = Status.CREATED;
            newManagerUserEditModel.ManagerId = ManagerEditModel.Id;
            newManagerUserEditModel.Manager = _applicationDbContext.Managers
                .Include(m => m.Users)
                .SingleOrDefault(m => m.Id == ManagerEditModel.Id);

            ManagerUserEditModels.Add(newManagerUserEditModel);

            return RedirectToPage("./Edit", newManagerUserEditModel.ManagerId);
        }

        public async Task<IActionResult> OnPostManagerUsersAsync()
        {

            var manager = _applicationDbContext.Managers
                .Include(m => m.Users)
                .FirstOrDefault(m => m.Id == ManagerUserEditModels[0].ManagerId);

            foreach (var manageruser in ManagerUserEditModels)
            {
                var manageruserold = manager.Users.Find(mu => mu.Id == manageruser.Id);
                manageruserold.Name = manageruser.Name;
                manageruserold.Token = manageruser.Token;
                manageruserold.SnmPv3Auth = manageruser.SnmPv3Auth;
                manageruserold.SnmPv3Priv = manageruser.SnmPv3Priv;
            }

            _applicationDbContext.Managers.Update(manager);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Edit", manager.Id);
        }
    }
}