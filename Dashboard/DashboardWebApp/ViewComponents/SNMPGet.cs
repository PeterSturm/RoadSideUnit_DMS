using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewComponents
{
    public class SNMPGet : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ViewModels.RSU.SnmpGETModel snmpget)
        {
            await Task.Delay(0);
            return View("GET", snmpget);
        }

    }
}
