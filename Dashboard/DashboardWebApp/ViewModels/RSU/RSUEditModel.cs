using DashboardWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewModels.RSU
{
    public class RSUEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Active { get; set; }
        [MinLength(0), MaxLength(32)]
        public string MIBVersion { get; set; }
        [MinLength(0), MaxLength(32)]
        public string FirmwareVersion { get; set; }
        [MinLength(0), MaxLength(140)]
        public string LocationDescription { get; set; }
        [MinLength(0), MaxLength(32)]
        public string Manufacturer { get; set; }
        public string NotificationIP { get; set; }
        public int NotificationPort { get; set; }

        public string ManagerIP { get; set; }
        public int ManagerPort { get; set; }
        public string prevMIP { get; set; }
        public int prevMPort { get; set; }

        public static RSUEditModel Parse(DashboardWebApp.Models.RSU rsu, DashboardWebApp.Models.Manager manager)
        {
            return new RSUEditModel
            {
                Id = rsu.Id,
                IP = rsu.IP.ToString(),
                Port = rsu.Port,
                Name = rsu.Name,
                Latitude = rsu.Latitude,
                Longitude = rsu.Longitude,
                Active = rsu.Active,
                MIBVersion = rsu.MIBVersion,
                FirmwareVersion = rsu.FirmwareVersion,
                LocationDescription = rsu.LocationDescription,
                Manufacturer = rsu.Manufacturer,
                NotificationIP = rsu.NotificationIP.ToString(),
                NotificationPort = rsu.NotificationPort,
                ManagerIP = manager.IP.ToString(),
                ManagerPort = manager.Port,
                prevMIP = manager.IP.ToString(),
                prevMPort = manager.Port
            };
        }

        public DashboardWebApp.Models.RSU MapToRSU(DashboardWebApp.Models.Manager manager)
        {
            return new DashboardWebApp.Models.RSU {
                Id = Id,
                IP = IPAddress.Parse(IP),
                Port = Port,
                Name = Name,
                Latitude = Latitude,
                Longitude = Longitude,
                Active = Active,
                MIBVersion = MIBVersion,
                FirmwareVersion = FirmwareVersion,
                LocationDescription = LocationDescription,
                Manufacturer = Manufacturer,
                NotificationIP = IPAddress.Parse(NotificationIP),
                NotificationPort = NotificationPort,
                Manager = manager
            };

        }
    }
}
