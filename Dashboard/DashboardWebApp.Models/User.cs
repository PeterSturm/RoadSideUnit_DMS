using Microsoft.AspNetCore.Identity;
using System;

namespace DashboardWebApp.Models
{
    public class User : IdentityUser
    {
        public string Token { get; set; }
    }
}
