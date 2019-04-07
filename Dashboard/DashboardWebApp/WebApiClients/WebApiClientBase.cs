using DashboardWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.WebApiClients
{
    public abstract class WebApiClientBase
    {
        protected string controller;

        public WebApiClientBase(string controller)
        {
            this.controller = controller;
        }

        protected string GetHost(ManagerUser managerUser)
        {
            return (managerUser.Manager.IP.ToString() == "127.0.0.1") ? "localhost" : managerUser.Manager.IP.ToString();
        }
    }
}
