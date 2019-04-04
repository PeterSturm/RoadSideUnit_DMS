using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DashboardWebApp.Models;
using System.Net;

namespace DashboardWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        //public DbSet<RSU> Rsus { get; set; }
        public DbSet<Manager> Managers { get; set; }
        //public DbSet<Settings> Settings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Manager>().HasKey(m => m.Id);
            //builder.Entity<Manager>().HasAlternateKey(m => new { m.IP, m.Port});

            Seed(builder);

            base.OnModelCreating(builder);
        }

        private void Seed(ModelBuilder builder)
        {
            builder.Entity<Manager>().HasData(
                new {
                    Id = 1,
                    Name = "Local",
                    IP = IPAddress.Parse("127.0.0.1"),
                    Port = 51467
                });
        }
    }
}
