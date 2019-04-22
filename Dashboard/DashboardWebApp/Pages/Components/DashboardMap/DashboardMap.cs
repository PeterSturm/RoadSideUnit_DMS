using DashboardWebApp.Models;
using DashboardWebApp.ViewModels;
using DashboardWebApp.ViewModels.RSU;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.Pages.Components.DashboardMap
{
    public class DashboardMap : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<MapNode> mapNodes)
        {
            await Task.Delay(0);
            return View("Map", mapNodes);
        }
    }
}
