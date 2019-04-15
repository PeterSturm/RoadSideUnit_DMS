using Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace DashboardWebApp.Models
{
    public class RSU
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IPAddress IP { get; set; }
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
        public IPAddress NotificationIP { get; set; }
        public int NotificationPort { get; set; }

        public Manager Manager { get; set; }

        public RsuDto ConvertToRSUDto()
        {
            return new RsuDto {
                Id = Id,
                IP = IP.ToString(),
                Port = Port,
                Name = Name,
                Latitude = Latitude,
                Longitude = Longitude,
                Active = Active, 
                MIBVersion = MIBVersion,
                FirmwareVersion = FirmwareVersion,
                LocationDescription = LocationDescription,
                Manufacturer = Manufacturer,
                NotificationIP = NotificationIP.ToString(),
                NotificationPort = NotificationPort
            };
        }

        public static RSU Parse(RsuDto rsudto, Manager manager)
        {
            return new RSU {
                Id = rsudto.Id,
                IP = IPAddress.Parse(rsudto.IP),
                Port = rsudto.Port,
                Name = rsudto.Name,
                Latitude = rsudto.Latitude,
                Longitude = rsudto.Longitude,
                Active = rsudto.Active,
                MIBVersion = rsudto.MIBVersion,
                FirmwareVersion = rsudto.FirmwareVersion,
                LocationDescription = rsudto.LocationDescription,
                Manufacturer = rsudto.Manufacturer,
                NotificationIP = IPAddress.Parse(rsudto.NotificationIP),
                NotificationPort = rsudto.NotificationPort,
                Manager = manager
            };
        }

    }
}
