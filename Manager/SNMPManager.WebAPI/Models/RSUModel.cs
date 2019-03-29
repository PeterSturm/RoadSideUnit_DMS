using SNMPManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SNMPManager.WebAPI.Models
{
    public class RSUModel
    {
        public int Id { get; set; }
        [Required]
        public string IP { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public bool Active { get; set; }
        [MinLength(0), MaxLength(32)]
        public string MIBVersion { get; set; }
        [MinLength(0), MaxLength(32)]
        public string FirmwareVersion { get; set; }
        [MinLength(0), MaxLength(140)]
        public string LocationDescription { get; set; }
        [MinLength(0), MaxLength(32)]
        public string Manufacturer { get; set; }
        [Required]
        public string NotificationIP { get; set; }
        public int NotificationPort { get; set; }

        public static RSUModel MaptoModel(RSU rsu)
        {
            return new RSUModel
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
                NotificationPort = rsu.Port
            };
        }

        public static RSU MaptoEntity(RSUModel rsumodel)
        {
            return new RSU
            {
                Id = rsumodel.Id,
                IP = IPAddress.Parse(rsumodel.IP),
                Port = rsumodel.Port,
                Name = rsumodel.Name,
                Latitude = rsumodel.Latitude,
                Longitude = rsumodel.Longitude,
                Active = rsumodel.Active,
                MIBVersion = rsumodel.MIBVersion,
                FirmwareVersion = rsumodel.FirmwareVersion,
                LocationDescription = rsumodel.LocationDescription,
                Manufacturer = rsumodel.Manufacturer,
                NotificationIP = IPAddress.Parse(rsumodel.NotificationIP),
                NotificationPort = rsumodel.Port
            };
        }
    }
}
