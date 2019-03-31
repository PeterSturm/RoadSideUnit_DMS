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

        public IList<RSU> RSUs { get; private set; }

        public async Task OnGetAsync()
        {
            var user = _applicationDbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user == null)
            {
                // TODO finish 
            }

            var manager = _applicationDbContext.Managers.FirstOrDefault();
            if (manager == null)
            {
                // TODO finish
            }

            var rsus = await _rsuService.GetAsync(manager, user);

            RSUs = rsus.ToList();
        }
    }
}