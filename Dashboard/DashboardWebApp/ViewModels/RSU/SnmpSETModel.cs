using DashboardWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewModels.RSU
{
    public class SnmpSETModel
    {
        public int ManagerId { get; set; }
        public int RsuId { get; set; }

        public string SET_OID { get; set; }
        public string SET_Type { get; set; }
        public string SET_Value { get; set; }

        public bool? Result { get; set; }
    }
}
