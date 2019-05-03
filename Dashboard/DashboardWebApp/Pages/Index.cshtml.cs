using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using DashboardWebApp.ViewModels;
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

        public class DashboardModel
        {
            public DashboardModel()
            {
                DownRSUs = new List<RSU>();
                MapNodes = new List<MapNode>();
            }
            public List<RSU> DownRSUs { get; set; }
            public List<MapNode> MapNodes { get; set; }
        }

        public DashboardModel Dashboard { get; set; }

        public async Task OnGetAsync()
        {
            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            Dashboard = new DashboardModel();

            var managers = _applicationDbContext.Managers
                .Include(m => m.Users)
                .ToList();
            if (managers != null)
            {
                // Fill the MapNodes to the Map component
                Dashboard.MapNodes = managers.Select(m => new MapNode(m.Name, $"{m.IP}/{m.Port}", m.Latitude, m.Longitude)).ToList();

                // Get all the Inactive RSUs for the DownRSU component
                foreach (var manager in managers)
                {
                    var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;
                    if (managerUser != null)
                    {
                        var rsus = await _rsuService.GetAsync(managerUser);
                        if (rsus != null)
                            Dashboard.DownRSUs.AddRange(rsus.Where(r => r.Active == false));
                    }
                }
            }
        }

        public IActionResult OnGetDownRsus()
        {
            List<RSU> downrsus = new List<RSU>();

            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            var managers = _applicationDbContext.Managers
                .Include(m => m.Users)
                .ToList();
            if (managers != null)
            {
                // Get all the Inactive RSUs for the DownRSU component
                foreach (var manager in managers)
                {
                    var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;
                    if (managerUser != null)
                    {
                        var rsus = _rsuService.GetAsync(managerUser).Result;
                        if (rsus != null)
                            downrsus.AddRange(rsus.Where(r => r.Active == false));
                    }
                }
            }
            return ViewComponent("DashboardDownRSUs", downrsus);
        }

        public JsonResult OnGetRsus(double lat, double lon)
        {
            var manager = _applicationDbContext.Managers
                .Include(m => m.Users)
                .Where(m => m.Latitude == lat && m.Longitude == lon)
                .FirstOrDefault();

            if (manager == null)
                return new JsonResult(null);

            var user = _applicationDbContext.Users
                .Include(u => u.UserManagerUsers)
                .FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            var managerUser = user.UserManagerUsers.FirstOrDefault(umu => umu.ManagerUserManagerId == manager.Id)?.ManagerUser;
            if(managerUser == null)
                return new JsonResult(null);

            var rsus = _rsuService.GetAsync(managerUser).Result;
            if(rsus == null)
                return new JsonResult(null);

            return new JsonResult(rsus
                .Select(r => new MapNode(r.Name, r.LocationDescription, r.Latitude, r.Longitude))
                .Select(mn => new { mn.Lat, mn.Lon})
                .ToArray());
        }
    }
}
