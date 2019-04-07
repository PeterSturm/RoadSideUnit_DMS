using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DashboardWebApp.Models
{
    public class User : IdentityUser
    {
        //public string Token { get; set; }
        public List<UserManagerUser> UserManagerUsers { get; set; }
    }
}
