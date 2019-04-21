using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexModel(RSUService rsuService, ApplicationDbContext applicationDbContext)
        {
            _rsuService = rsuService;
            _applicationDbContext = applicationDbContext;
        }

        public List<RSU> DownRSUs { get; set; }

        public async Task OnGetAsync()
        {
            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            DownRSUs = new List<RSU>();

            var managers = _applicationDbContext.Managers
                .Include(m => m.Users)
                .ToList();
            if (managers == null)
                NotFound("There are no managers");

            foreach (var manager in managers)
            {
                var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;
                if (managerUser != null)
                {
                    var rsus = await _rsuService.GetAsync(managerUser);
                    if (rsus != null)
                        DownRSUs.AddRange(rsus.Where(r => r.Active == false));
                }
            }
        }
    }
}
