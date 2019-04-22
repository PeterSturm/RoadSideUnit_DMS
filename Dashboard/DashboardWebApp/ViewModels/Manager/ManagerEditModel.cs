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
        public List<ManagerUserEditModel> ManagerUserEditModels { get; set; }

        public static ManagerEditModel Parse(Models.Manager manager)
        {
            return new ManagerEditModel
            {
                Id = manager.Id,
                Name = manager.Name,
                IP = manager.IP,
                Port = manager.Port,
                Latitude = manager.Latitude,
                Longitude = manager.Longitude,
                Users = manager.Users,
                ManagerUserEditModels = manager.Users.Select(mu => new ManagerUserEditModel(mu)).ToList()
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
                manager.Latitude = manager.Latitude;
                manager.Longitude = manager.Longitude;
                manager.Users = Users;
            }
            else
            {
                manager = new Models.Manager
                {
                    Name = Name,
                    IP = IP,
                    Port = Port,
                    Users = Users,
                    Latitude = Latitude,
                    Longitude = Longitude
                };
            }

            return manager;
        }
    }
}
