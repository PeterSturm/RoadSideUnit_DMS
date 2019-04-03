using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewModels.RSU
{
    public class RSUAddModel : DashboardWebApp.Models.RSU
    {
        public string _IP { get { return IP.ToString(); } set { IP = IPAddress.Parse(value); } }
        public string _NotificationIP { get { return NotificationIP.ToString(); } set { NotificationIP = IPAddress.Parse(value); } }
        public int ManagerId { get; set; }
    }
}
