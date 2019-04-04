using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DashboardWebApp.Models;
using Microsoft.AspNetCore.Identity;
using DashboardWebApp.Data;

namespace DashboardWebApp.Pages.RSUs
{
    public class IndexModel : PageModel
    {
        private readonly IRSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexModel(IRSUService rsuService, ApplicationDbContext applicationDbContext)
        {
            _rsuService = rsuService;
            _applicationDbContext = applicationDbContext;
        }

        public List<RSU> RSUs { get; private set; }

        public async Task OnGetAsync(int? managerId)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            RSUs = new List<RSU>();

            if (managerId.HasValue)
            {
                var manager = _applicationDbContext.Managers.FirstOrDefault(m => m.Id == managerId);
                if (manager == null)
                {
                    NotFound($"No manager with id: {managerId}");
                }

                var rsus = await _rsuService.GetAsync(manager, user);
                if(rsus != null)
                    RSUs = rsus.ToList();
            }
            else
            {
                var managers = _applicationDbContext.Managers.ToList();
                if (managers == null)
                    NotFound("There are no managers");

                foreach (var manager in managers)
                {
                    var rsus = await _rsuService.GetAsync(manager, user);
                    if (rsus != null)
                        RSUs.AddRange(rsus);
                }
            }
        }
    }
}