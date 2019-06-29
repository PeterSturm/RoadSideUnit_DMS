using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.ViewModels.Manager;
using DashboardWebApp.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebApp.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EditModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public UserViewModel UserView { get; set; }

        [BindProperty]
        public AddManagerUserModel AddManagerUser { get; set; }

        public class AddManagerUserModel
        {            
            public int ManagerUserManagerId { get; set; }
            public int ManagerUserUserId { get; set; }
            public List<SelectListItem> Managers { get; set; }
            public List<SelectListItem> ManagerUsers { get; set; }
        }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = _applicationDbContext.Users
                .SingleOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"User not found with id {id}");

            UserView = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            var usermanagerusers = _applicationDbContext.UserManagerUsers
                .Where(umu => umu.UserId == user.Id)
                ?.Include(umu => umu.ManagerUser)
                    ?.ThenInclude(mu => mu.Manager)
                .ToList();
            if (usermanagerusers != null)
            {
                var managerusers = usermanagerusers.Select(umu => umu.ManagerUser).ToList();
                UserView.ManagerUsers = managerusers;
            }

            AddManagerUser = InitAddManagerUserModel();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var manageruserName = await _applicationDbContext.ManagerUsers.FirstOrDefaultAsync(mu => mu.ManagerId == AddManagerUser.ManagerUserManagerId && mu.Id == AddManagerUser.ManagerUserUserId);
            if (manageruserName == null)
                return NotFound($"Not found manager user with id {AddManagerUser.ManagerUserUserId} in manager {AddManagerUser.ManagerUserManagerId}");

            UserManagerUser userManagerUser = new UserManagerUser();
            userManagerUser.ManagerUserManagerId = AddManagerUser.ManagerUserManagerId;
            userManagerUser.ManagerUserName = manageruserName.Name;
            userManagerUser.UserId = UserView.Id;

            _applicationDbContext.UserManagerUsers.Add(userManagerUser);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = UserView.Id});
        }

        private AddManagerUserModel InitAddManagerUserModel()
        {
            AddManagerUserModel addManagerUser = new AddManagerUserModel();

            addManagerUser.Managers = _applicationDbContext.Managers.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = $"{m.Name} {m.IP}/{m.Port}"
            })
            .ToList();

            addManagerUser.ManagerUserManagerId = int.Parse(addManagerUser.Managers.First().Value);
            addManagerUser.ManagerUsers = new List<SelectListItem>();

            return addManagerUser;
        }

        public JsonResult OnGetManagerUsers(int managerId)
        {
            var managerusers = _applicationDbContext.ManagerUsers.Where(mu => mu.ManagerId == managerId);
            if (managerusers != null && managerusers.Count() != 0)
            {
                var manageruserlist = managerusers.Select(mu => new SelectListItem
                {
                    Value = mu.Id.ToString(),
                    Text = $"{mu.Name} - {mu.Role}"
                })
                .ToList();

                return new JsonResult(manageruserlist);
            }

            return new JsonResult(null);
        }

        /*public async Task<IActionResult> AddManagerUserToUser()
        {

        }*/
    }
}