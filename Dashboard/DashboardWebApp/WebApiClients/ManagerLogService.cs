using DashboardWebApp.Models;
using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DashboardWebApp.WebApiClients
{
    public class ManagerLogService : WebApiClientBase
    {
        private readonly HttpClient _httpClinet;

        public ManagerLogService(HttpClient httpClinet) : base("api/managerlog")
        {
            _httpClinet = httpClinet;
        }

        public async Task<IEnumerable<ManagerLog>> GetAsync(ManagerUser managerUser)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}");
                var managerlogs = ManagerLogDto.FromJsonCollection(result);


                return managerlogs.Select(m => ManagerLog.Parse(m, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ManagerLog>> GetAsync(ManagerUser managerUser, DateTime from, DateTime to)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{from.ToLongDateString()}/{to.ToLongDateString()}");
                var managerlogs = ManagerLogDto.FromJsonCollection(result);


                return managerlogs.Select(m => ManagerLog.Parse(m, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ManagerLog>> GetAsync(ManagerUser managerUser, string type, DateTime from, DateTime to)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{type}/{from.ToLongDateString()}/{to.ToLongDateString()}");
                var managerlogs = ManagerLogDto.FromJsonCollection(result);


                return managerlogs.Select(m => ManagerLog.Parse(m, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }
    }
}
