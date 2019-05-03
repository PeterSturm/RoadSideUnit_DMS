using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DashboardWebApp;

namespace DashboardWebApp.ViewModels.RSU
{
    public class RSUDetail : Models.RSU
    {
        public Models.RSU Rsu { get; set; }

        public double Elevation { get; set; }
        public int FrequencyDefault { get; set; }
        public int FrequencySecondary { get; set; }
        public int BandwidthDefault { get; set; }
        public int BandwidthSecondary { get; set; }
        public bool Cam { get; set; }
        public bool Denm { get; set; }
        public bool Ldm { get; set; }

    }
}
