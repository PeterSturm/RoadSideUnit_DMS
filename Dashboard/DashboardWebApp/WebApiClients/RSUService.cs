using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DashboardWebApp.Models;
using DTO;

namespace DashboardWebApp.WebApiClients
{
    public class RSUService : WebApiClientBase
    {
        private readonly HttpClient _httpClinet;

        public RSUService(HttpClient httpClinet) : base("api/rsu")
        {
            _httpClinet = httpClinet;
        }

        public async Task<IEnumerable<RSU>> GetAsync(ManagerUser managerUser)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}");
                var rsus = RsuDto.FromJsonCollection(result);


                return rsus.Select(r => RSU.Parse(r, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<RSU> GetAsync(ManagerUser managerUser, string IP, int port)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{IP}/{port}");
                var rsu = RsuDto.FromJson(result);

                return RSU.Parse(rsu, managerUser.Manager);
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<RSU> GetAsync(ManagerUser managerUser, int id)
        {
            var host = GetHost(managerUser);

            try
            { 
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{id}");
                var rsu = RsuDto.FromJson(result);

                return RSU.Parse(rsu, managerUser.Manager);
            }
            catch (HttpRequestException ex)
            {
                return null;
            }

        }

        public async Task<bool> AddAsync(ManagerUser managerUser, RSU rsu)
        {
            var host = GetHost(managerUser);

            RsuDto rsuDto = rsu.ConvertToRSUDto();

            try
            {
                var result = await _httpClinet.PostAsJsonAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}", rsuDto);

                return true;
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ManagerUser managerUser, RSU rsu)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.PutAsJsonAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}", rsu.ConvertToRSUDto());
                return true;
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(ManagerUser managerUser, int rsuId)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.DeleteAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{rsuId}");
                return true;
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }
    }
}
