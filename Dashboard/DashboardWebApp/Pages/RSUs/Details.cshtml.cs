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
    public class DetailsModel : PageModel
    {

        private readonly SnmpService _snmpService;
        private readonly RSUService _rsuService;
        private readonly ApplicationDbContext _applicationDbContext;

        public DetailsModel(SnmpService snmpService, ApplicationDbContext applicationDbContext, RSUService rsuService)
        {
            _snmpService = snmpService;
            _applicationDbContext = applicationDbContext;
            _rsuService = rsuService;
        }

        public RSUDetail RSU { get; set; }

        [BindProperty]
        public SnmpGETModel snmpGETModel { get; set; }

        public async Task OnGetAsync(int managerId, int rsuId)
        {
            snmpGETModel = new SnmpGETModel();
            snmpGETModel.ManagerId = managerId;
            snmpGETModel.RsuId = rsuId;

            await LoadRSUData(managerId, rsuId);
        }

        private async Task LoadRSUData(int managerId, int rsuId)
        {
            RSU = new RSUDetail();

            var user = await _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
            var manager = await _applicationDbContext.Managers
                .Include(m => m.Users)
                .FirstOrDefaultAsync(m => m.Id == managerId);
            if (manager == null)
            {
                NotFound($"No manager with id: {managerId}");
            }
            var manageruser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;

            RSU.Rsu = await _rsuService.GetAsync(manageruser, rsuId);

            if (RSU.Rsu != null)
            {
                bool timeout = SetData(manageruser, rsuId, "0.1.15628.4.1.8.8", RSU.Elevation);
                if (!timeout) timeout = SetData(manageruser, rsuId, "0.1.15628.4.1.8.9", RSU.FrequencyDefault);
                if (!timeout) timeout = SetData(manageruser, rsuId, "0.1.15628.4.1.8.10", RSU.FrequencySecondary);
                if (!timeout) timeout = SetData(manageruser, rsuId, "0.1.15628.4.1.8.13", RSU.BandwidthDefault);
                if (!timeout) timeout = SetData(manageruser, rsuId, "0.1.15628.4.1.8.14", RSU.BandwidthSecondary);
                if (!timeout) timeout = SetData(manageruser, rsuId, "0.1.15628.4.1.8.15", RSU.Cam);
                if (!timeout) timeout = SetData(manageruser, rsuId, "0.1.15628.4.1.8.16", RSU.Denm);
                if (!timeout) timeout = SetData(manageruser, rsuId, "0.1.15628.4.1.8.20", RSU.Ldm);
            }
        }

        private bool SetData(ManagerUser manageruser, int rsuId, string OID, Object data)
        {
            MIBObject mibo = _snmpService.GetAsync(manageruser, rsuId, "0.1.15628.4.1.8.8").Result;

            if (mibo != null)
            {
                if (mibo.Value.Equals("Timeout"))
                    return true;

                if (data is int)
                {
                    int value = (int.TryParse(mibo.Value, out value)) ? value : 0;
                    data = value;
                }
                else if (data is double)
                {
                    double value = (double.TryParse(mibo.Value, out value)) ? value / 1000000 : 0.0;
                    data = value;
                }
                else if (data is string)
                {
                    data = mibo.Value;
                }
            }

            return false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadRSUData(snmpGETModel.ManagerId, snmpGETModel.RsuId);

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