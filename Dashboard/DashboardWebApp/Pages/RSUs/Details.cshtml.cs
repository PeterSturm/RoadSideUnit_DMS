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

        public RSUDetail RSUDetail { get; set; }

        [BindProperty]
        public SnmpGETModel snmpGETModel { get; set; }
        [BindProperty]
        public SnmpSETModel snmpSETModel { get; set; }

        public async Task OnGetAsync(int managerId, int rsuId)
        {
            snmpGETModel = new SnmpGETModel();
            snmpGETModel.ManagerId = managerId;
            snmpGETModel.RsuId = rsuId;

            snmpSETModel = new SnmpSETModel();
            snmpSETModel.ManagerId = managerId;
            snmpSETModel.RsuId = rsuId;

            await LoadRSUData(managerId, rsuId);
        }


        public async Task<IActionResult> OnPostGETAsync()
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
            ResetSetModel();

            return Page();
        }

        public async Task<IActionResult> OnPostSETAsync()
        {
            await LoadRSUData(snmpSETModel.ManagerId, snmpSETModel.RsuId);

            if (string.IsNullOrEmpty(snmpSETModel.SET_OID))
                return Page();

            var user = await _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
            var manager = await _applicationDbContext.Managers
                .Include(m => m.Users)
                .FirstOrDefaultAsync(m => m.Id == snmpSETModel.ManagerId);
            if (manager == null)
            {
                NotFound($"No manager with id: {snmpSETModel.ManagerId}");
            }
            var manageruser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;

            MIBObject mibo = new MIBObject();
            mibo.OID = snmpSETModel.SET_OID;
            mibo.Type = snmpSETModel.SET_Type;
            mibo.Value = snmpSETModel.SET_Value;

            snmpSETModel.Result = await _snmpService.SetAsync(manageruser, snmpSETModel.RsuId, mibo);
            snmpSETModel.SET_OID = "";
            snmpSETModel.SET_Type = "";
            snmpSETModel.SET_Value = "";
            ResetGetModel();

            await LoadRSUData(snmpSETModel.ManagerId, snmpSETModel.RsuId);

            return Page();
        }

        private void ResetGetModel()
        {
            snmpGETModel.OID = "";
            snmpGETModel.Type = "";
            snmpGETModel.Value = "";
        }

        private void ResetSetModel()
        {
            snmpSETModel.SET_OID = "";
            snmpSETModel.SET_Type = "";
            snmpSETModel.SET_Value = "";
            snmpSETModel.Result = null;
        }

        private async Task LoadRSUData(int managerId, int rsuId)
        {
            RSUDetail = new RSUDetail();

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

            RSUDetail.Rsu = await _rsuService.GetAsync(manageruser, rsuId);

            if (RSUDetail.Rsu != null)
            {
                object temp = SetData(manageruser, rsuId, "0.1.15628.4.1.8.8");
                if (temp != null)
                {
                    RSUDetail.Elevation = ((double) (int)temp) / 1000000;
                    temp = SetData(manageruser, rsuId, "0.1.15628.4.1.8.9");
                }
                if (temp != null)
                {
                    RSUDetail.FrequencyDefault = (int)temp;
                    temp = SetData(manageruser, rsuId, "0.1.15628.4.1.8.10");
                }
                if (temp != null)
                {
                    RSUDetail.FrequencySecondary = (int)temp;
                    temp = SetData(manageruser, rsuId, "0.1.15628.4.1.8.13");
                }
                if (temp != null)
                {
                    RSUDetail.BandwidthDefault = (int)temp;
                    temp = SetData(manageruser, rsuId, "0.1.15628.4.1.8.14");
                }
                if (temp != null)
                {
                    RSUDetail.BandwidthSecondary = (int)temp;
                    temp = SetData(manageruser, rsuId, "0.1.15628.4.1.9.15");
                }
                if (temp != null)
                {
                    RSUDetail.Cam = StringToBool((string)temp);
                    temp = SetData(manageruser, rsuId, "0.1.15628.4.1.9.16");
                }
                if (temp != null)
                {
                    RSUDetail.Denm = StringToBool((string)temp);
                    temp = SetData(manageruser, rsuId, "0.1.15628.4.1.9.20");
                }
                if (temp != null)
                {
                    RSUDetail.Ldm = StringToBool((string)temp);
                }
            }
        }

        private bool StringToBool(string value)
        {
            switch (value)
            {
                case "Y": return true;
                case "N": return false;
                default: return false;
            }
        }

        private object SetData(ManagerUser manageruser, int rsuId, string OID)
        {
            MIBObject mibo = _snmpService.GetAsync(manageruser, rsuId, OID).Result;
            
            if (mibo != null)
            {
                if (mibo.Value.Equals("Timeout"))
                    return null;

                if (mibo.Type == "Integer32" || mibo.Type == "Counter32" || mibo.Type == "Integer")
                {
                    int value = (int.TryParse(mibo.Value, out value)) ? value : 0;
                    return value;
                }
                /*else if (data is double)
                {
                    double value = (double.TryParse(mibo.Value, out value)) ? value / 1000000 : 0.0;
                    return value;
                }*/
                else if (mibo.Type == "OctetString")
                {
                    return mibo.Value;
                }
            }

            return null;
        }
    }
}