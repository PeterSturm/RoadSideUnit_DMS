using DashboardWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewModels.RSU
{
    public class SnmpGETModel
    {
        public int ManagerId { get; set; }
        public int RsuId { get; set; }

        public string OID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
