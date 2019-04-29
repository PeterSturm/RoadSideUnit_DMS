using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.ViewModels.RSU;
using DashboardWebApp.WebApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebApp.Pages.RSUs
{
    public class SnmpGET : PageModel
    {
        private readonly SnmpService _snmpService;
        private readonly ApplicationDbContext _applicationDbContext;

        public SnmpGET(SnmpService snmpService, ApplicationDbContext applicationDbContext)
        {
            _snmpService = snmpService;
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public SnmpGETModel snmpGETModel { get; set; }

        public async Task OnGetAsync(int managerId, int rsuId)
        {
            snmpGETModel = new SnmpGETModel();
            snmpGETModel.ManagerId = managerId;
            snmpGETModel.RsuId = rsuId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(snmpGETModel.OID))
                return Page();

            var user = await _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
            var manager = await _applicationDbContext.Managers
                .Include(m => m.Users)
                .FirstOrDefaultAsync(m => m.Id == snmpGETModel.ManagerId);
            if (manager == null)
            {
                NotFound($"No manager with id: {snmpGETModel.ManagerId}");
            }
            var manageruser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;

            MIBObject mibo = await _snmpService.GetAsync(manageruser, snmpGETModel.RsuId, snmpGETModel.OID);

            snmpGETModel.Type = mibo.Type;
            snmpGETModel.Value = mibo.Value;

            return Page();
        }
    }
}