using System;
using System.Collections.Generic;
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
        public Manager Manager { get; set; }
    }
}
