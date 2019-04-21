using DashboardWebApp.Models;
using DashboardWebApp.ViewModels.RSU;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewComponents
{
    public class DashboardDownRSUs : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<RSU> rsus)
        {
            await Task.Delay(0);
            return View("DownRsus", rsus);
        }
    }
}
