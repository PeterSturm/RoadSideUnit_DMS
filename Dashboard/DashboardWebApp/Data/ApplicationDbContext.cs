using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DashboardWebApp.Models;
using System.Net;

namespace DashboardWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Manager> Managers { get; set; }
        public DbSet<ManagerUser> ManagerUsers { get; set; }
        public DbSet<UserManagerUser> UserManagerUsers { get; set; }
        //public DbSet<Settings> Settings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Manager>().HasKey(m => m.Id);

            builder.Entity<ManagerUser>().HasKey(mu => new { mu.ManagerId, mu.Name });
            builder.Entity<ManagerUser>()
                .HasOne(mu => mu.Manager)
                .WithMany(m => m.Users)
                .HasForeignKey(mu => mu.ManagerId)
                .HasConstraintName("ForeignKey_ManagerUser_Manager");

            builder.Entity<UserManagerUser>().HasKey(umu => new
            {
                umu.UserId,
                umu.ManagerUserManagerId,
                umu.ManagerUserName
            });
            builder.Entity<UserManagerUser>()
                .HasOne(umu => umu.User)
                .WithMany(mu => mu.UserManagerUsers)
                .HasForeignKey(umu => umu.UserId);
            builder.Entity<UserManagerUser>()
                .HasOne(umu => umu.ManagerUser)
                .WithMany(mu => mu.UserManagerUsers)
                .HasForeignKey(umu => new { umu.ManagerUserManagerId, umu.ManagerUserName });

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

            /*builder.Entity<ManagerUser>().HasData(new
            {              
                ManagerId = 1,
                Name = "sturm",
                Token = "test"
            });*/

            /*builder.Entity<UserManagerUser>().HasData(new
            {
                UserUserName = "Admin",
                ManagerUserManagerId = 1,
                ManagerUserName = "sturm"
            });*/
        }
    }
}
