using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SNMPManager.Core.Entities;

namespace SNMPManager.Persistence
{
    public class ManagerContext : DbContext
    {
        public DbSet<RSU> RSUs { get; set; }
        public DbSet<ManagerLog> ManagerLogs { get; set; }
        public DbSet<TrapLog> TrapLogs { get; set; }
        public DbSet<LogType> LogTypes { get; set; }
        public DbSet<User> users { get; set; }

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setup composite keys for the log tables
            modelBuilder.Entity<ManagerLog>().HasKey(log => new { log.TimeStamp, log.TypeId });
            modelBuilder.Entity<TrapLog>().HasKey(log => new { log.TimeStamp, log.TypeId, log.SourceRSU});

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
