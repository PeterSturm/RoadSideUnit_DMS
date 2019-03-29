using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNMPManager.Core.Entities
{
    public class RSU
    {
        public int Id { get; set; }
        [Required]
        public IPAddress IP { get; set; }
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
        public IPAddress NotificationIP { get; set; }
        public int NotificationPort { get; set; }

    }
}
