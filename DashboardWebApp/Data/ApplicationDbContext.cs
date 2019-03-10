using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DashboardWebApp.Models;

namespace DashboardWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RSU> Rsus { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
