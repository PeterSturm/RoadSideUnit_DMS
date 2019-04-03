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
    public class TrapListener : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private Listener listener;
        private readonly string userName;
        private readonly string authPass;
        private readonly string privPass;
        private readonly IPAddress ownIP;
        private readonly int listenerPort;

        public TrapListener(IServiceProvider serviceProvider, string username, string authpass, string privpass, string ip, int port)
        {
            _serviceProvider = serviceProvider;
            userName = username;
            authPass = authpass;
            privPass = privpass;
            ownIP = IPAddress.Parse(ip);
            listenerPort = port;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var users = new UserRegistry();
            users.Add(new OctetString(userName),
                      new DESPrivacyProvider(new OctetString(privPass), new SHA1AuthenticationProvider(new OctetString(authPass))));
            listener = new Listener { Users = users };
            listener.AddBinding(new IPEndPoint(IPAddress.Any, listenerPort));
            listener.MessageReceived += MessageReceived;
            listener.ExceptionRaised += ExceptionHandler;
            listener.StartAsync();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            listener.Stop();

            return Task.CompletedTask;
        }

        private void ExceptionHandler(object sender, ExceptionRaisedEventArgs e)
        {
            var message = e.Exception.Message;
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using ( var scope = _serviceProvider.CreateScope())
            {
                var contextService = scope.ServiceProvider.GetRequiredService<IContextService>();

                int? rsu = contextService.GetRSU().SingleOrDefault(r => r.IP.ToString() == e.Sender.Address.ToString()
                                                                     && r.Port == e.Sender.Port)?.Id;

                // Registrate new RSU +
                if (!rsu.HasValue)
                { 
                    bool success = contextService.AddRSU(new RSU
                                    {
                                        Name = "Unnamed",
                                        IP = e.Sender.Address,
                                        Port = e.Sender.Port,
                                        //Latitude = 0.0,
                                        //Longitude = 0.0, 
                                        Active = true,
                                        MIBVersion = "unknown",
                                        FirmwareVersion = "unknown",
                                        LocationDescription = "unknown",
                                        Manufacturer = "unknown",
                                        NotificationIP = ownIP,
                                        NotificationPort = listenerPort
                                    });
                    if (success)
                    {
                        rsu = contextService.GetRSU().Single(r => r.IP == e.Sender.Address && r.Port == e.Sender.Port).Id;
                        contextService.AddManagerLog(new ManagerLog(DateTime.Now, LogType.DB, $"Registered new RSU with id: {rsu}"));
                    }
                    else
                    {
                        contextService.AddManagerLog(new ManagerLog(DateTime.Now, LogType.DB, $"Registering new RSU({e.Sender.Address.ToString()}/{e.Sender.Port}) failed"));
                        return;
                    }
                }
                // Registrate new RSU -

                if (e.Message.Parameters.IsInvalid)
                    contextService.AddTrapLog(new TrapLog(DateTime.Now, LogType.SNMP, rsu.GetValueOrDefault(), "Invalid Trap mesage!"));

                try
                {
                    if (e.Message.Scope.Pdu.Variables.Count == 0)
                        contextService.AddTrapLog(new TrapLog(DateTime.Now, LogType.SNMP, rsu.GetValueOrDefault(), "Heartbeat ping"));

                    /*if (e.Message.Scope.Pdu.TypeCode == SnmpType.TrapV2Pdu)
                    {
                        TrapV2Pdu v2trap = (TrapV2Pdu) e.Message.Scope.Pdu;
                        var variables = v2trap.Variables.ToList();
                        //contextService.AddTrapLog(new TrapLog(DateTime.Now, LogType.SNMP, rsu.GetValueOrDefault(), v2trap.Variables..ToString()));
                    }*/

                }
                catch(Exception ex)
                {
                    contextService.AddTrapLog(new TrapLog(DateTime.Now, LogType.SNMP, rsu.GetValueOrDefault(), $"Error processing Trap message! {ex.Message.Take(250)}"));
                }
            }
        }
    }
}
