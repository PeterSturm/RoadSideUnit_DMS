using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewComponents
{
    public class SNMPSet : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ViewModels.RSU.SnmpSETModel snmpset)
        {
            await Task.Delay(0);
            return View("SET", snmpset);
        }

    }
}
