using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;

namespace SNMPManager.Persistence
{
    public class ManagerContext : DbContext
    {
        public DbSet<RSU> RSUs { get; set; }
        public DbSet<ManagerLog> ManagerLogs { get; set; }
        public DbSet<TrapLog> TrapLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ManagerSettings> ManagerSettings{get; set;}

        public ManagerContext() => CreateMappers();
        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options) => CreateMappers();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlHasEnum<LogType>();

            // Setup composite keys for the log tables
            modelBuilder.Entity<User>().HasAlternateKey(user => user.UserName);
            modelBuilder.Entity<ManagerLog>().HasKey(log => new { log.TimeStamp, log.Type });
            modelBuilder.Entity<TrapLog>().HasKey(log => new { log.TimeStamp, log.Type, log.SourceRSU});

            Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            CreateMappers();
            base.OnConfiguring(optionsBuilder);
        }

        private void CreateMappers()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<LogType>();
        }

        private void Seed(ModelBuilder builder)
        {
            builder.Entity<ManagerSettings>().HasData(
                new {
                    Id = 1,
                    Timeout = 2000
                });
            builder.Entity<Role>().HasData(
                new
                {
                    Id = 1,
                    Name = "Admin"
                },
                new
                {
                    Id = 2,
                    Name = "Monitor"
                });

            builder.Entity<User>().HasData(
                new
                {
                    Id = 1,
                    UserName = "admin",
                    Token = "Adminpass01",
                    RoleId = 1,
                    SNMPv3Auth = "authpass012",
                    SNMPv3Priv = "privpass012",

                },
                new
                {
                    Id = 2,
                    UserName = "monitor",
                    Token = "Monitorpass01",
                    RoleId = 2,
                    SNMPv3Auth = "authpass012",
                    SNMPv3Priv = "privpass012",

                });

            builder.Entity<RSU>().HasData(
                new RSU
                {
                    Id = 1,
                    IP = IPAddress.Parse("172.168.45.27"),
                    Port = 162,
                    Name = "TestRSU",
                    Latitude = 17.45,
                    Longitude = 24.12,
                    Active = true,
                    MIBVersion = "",
                    FirmwareVersion = "",
                    LocationDescription = "",
                    Manufacturer = "Commsignia",
                    NotificationIP = IPAddress.Parse("186.56.123.84"),
                    NotificationPort = 161
                },
                new RSU
                {
                    Id = 2,
                    IP = IPAddress.Parse("112.111.45.89"),
                    Port = 162,
                    Name = "RSUuu",
                    Latitude = 19.45,
                    Longitude = 45.12,
                    Active = true,
                    MIBVersion = "",
                    FirmwareVersion = "",
                    LocationDescription = "",
                    Manufacturer = "Commsignia",
                    NotificationIP = IPAddress.Parse("186.56.123.84"),
                    NotificationPort = 161
                },
                new RSU
                {
                    Id = 3,
                    IP = IPAddress.Parse("127.0.0.1"),
                    Port = 161,
                    Name = "RSUjavaagent",
                    Latitude = 13.45,
                    Longitude = 32.12,
                    Active = true,
                    MIBVersion = "",
                    FirmwareVersion = "",
                    LocationDescription = "",
                    Manufacturer = "Commsignia",
                    NotificationIP = IPAddress.Parse("127.0.0.1"),
                    NotificationPort = 162
                });
        }
    }
}
