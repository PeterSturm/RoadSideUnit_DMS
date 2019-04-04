using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewModels.Manager
{
    public class ManagerEditModel : Models.Manager
    {
        public string ip {
            get { return IP.ToString(); }
            set { IP = IPAddress.Parse(value); }
        }

        public static ManagerEditModel Parse(Models.Manager manager)
        {
            return new ManagerEditModel
            {
                Id = manager.Id,
                Name = manager.Name,
                IP = manager.IP,
                Port = manager.Port
            };
        }

        public Models.Manager MapToManager(Models.Manager manager)
        {
            if (manager != null)
            {
                manager.Id = Id;
                manager.Name = Name;
                manager.IP = IP;
                manager.Port = Port;
            }
            else
            {
                manager = new Models.Manager
                {
                    Name = Name,
                    IP = IP,
                    Port = Port
                };
            }

            return manager;
        }
    }
}
