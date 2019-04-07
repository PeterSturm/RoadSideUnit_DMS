using DashboardWebApp.Models;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DashboardWebApp.WebApiClients
{
    public class TrapLogService : WebApiClientBase
    {
        private readonly HttpClient _httpClinet;

        public TrapLogService(HttpClient httpClinet) : base("api/traplog")
        {
            _httpClinet = httpClinet;
        }

        public async Task<IEnumerable<TrapLog>> GetAsync(ManagerUser managerUser)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}");
                var traplogs = TrapLogDto.FromJsonCollection(result);


                return traplogs.Select(t => TrapLog.Parse(t, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TrapLog>> GetAsync(ManagerUser managerUser, DateTime from, DateTime to)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{from.ToLongDateString()}/{to.ToLongDateString()}");
                var traplogs = TrapLogDto.FromJsonCollection(result);


                return traplogs.Select(t => TrapLog.Parse(t, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TrapLog>> GetAsync(ManagerUser managerUser, string type, DateTime from, DateTime to)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{type}/{from.ToLongDateString()}/{to.ToLongDateString()}");
                var TrapLogs = TrapLogDto.FromJsonCollection(result);


                return TrapLogs.Select(t => TrapLog.Parse(t, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TrapLog>> GetAsync(ManagerUser managerUser, int rsuId, DateTime from, DateTime to)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{rsuId}/{from.ToLongDateString()}/{to.ToLongDateString()}");
                var TrapLogs = TrapLogDto.FromJsonCollection(result);


                return TrapLogs.Select(t => TrapLog.Parse(t, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<TrapLog>> GetAsync(ManagerUser managerUser, int rsuId)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{rsuId}");
                var TrapLogs = TrapLogDto.FromJsonCollection(result);


                return TrapLogs.Select(t => TrapLog.Parse(t, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }
    }
}
