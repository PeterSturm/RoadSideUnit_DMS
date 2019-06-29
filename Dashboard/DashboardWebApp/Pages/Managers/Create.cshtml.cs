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
        private readonly UserService _userService;

        public CreateModel(ApplicationDbContext applicationDbContext, UserService userService)
        {
            _applicationDbContext = applicationDbContext;
            _userService = userService;
        }

        [BindProperty]
        public ManagerEditModel Manager { get; set; }


        public void OnGet()
        {
            Page();
        }

        public async Task<IActionResult> OnPostManagerEditAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Manager manager = Manager.MapToManager(null);

            var managerUsers = await _userService.GetAsync(new ManagerUser(manager));
            if (managerUsers != null)
                _applicationDbContext.ManagerUsers.AddRange(managerUsers);
            else
                return Page();

            manager.Users = managerUsers?.ToList();

            _applicationDbContext.Managers.Add(manager);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}