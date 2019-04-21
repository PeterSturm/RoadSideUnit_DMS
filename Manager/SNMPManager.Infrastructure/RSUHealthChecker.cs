using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SNMPManager.Core.Interfaces;
using SNMPManager.Core.Entities;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using SNMPManager.Core.Enumerations;
using System.Linq;

namespace SNMPManager.Infrastructure
{
    public class RSUHealthChecker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer timer;

        public RSUHealthChecker(IServiceProvider serviceProvider)
        { 
            _serviceProvider = serviceProvider;
        }

        private void HealthCheck(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var contextService = scope.ServiceProvider.GetRequiredService<IContextService>();

                var rsus = contextService.GetRSU().Where(r => r.Active);
                foreach (var rsu in rsus)
                {
                    var lastlog = contextService.GetTrapLogs(rsu.Id)?.LastOrDefault();
                    if (lastlog != null)
                    {
                        if ((DateTime.Now - lastlog.TimeStamp).TotalMinutes > 5)
                        {
                            rsu.Active = false;
                            contextService.UpdateRSU(rsu);
                        }
                    }
                    else
                    {
                        rsu.Active = false;
                        contextService.UpdateRSU(rsu);
                    }
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            timer = new Timer(HealthCheck, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            timer?.Dispose();
            base.Dispose();
        }
    }
}
