using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardWebApp.Data;
using DashboardWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DashboardWebApp.Pages.Managers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IList<Manager> Managers { get; private set; }

        public void OnGet()
        {
            var managers =  _applicationDbContext.Managers.ToList();
            if (managers != null)
                Managers = managers;
        }
    }
}