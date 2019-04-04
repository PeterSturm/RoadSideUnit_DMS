using DashboardWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.WebApiClients
{
    public interface IRSUService
    {
        Task<IEnumerable<RSU>> GetAsync(Manager manager, User user);
        Task<RSU> GetAsync(Manager manager, User user,string IP, int port);
        Task<RSU> GetAsync(Manager manager, User user,int id);

        Task<bool> AddRSUAsync(Manager manager, User user,RSU rsu);

        Task<bool> UpdateRSUAsync(Manager manager, User user,RSU rsu);

        Task<bool> DeleteRSUAsync(Manager manager, User user,int rsuId);
    }
}
