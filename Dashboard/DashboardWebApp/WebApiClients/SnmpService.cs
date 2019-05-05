using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DashboardWebApp.Models;
using Common.DTO;

namespace DashboardWebApp.WebApiClients
{
    public class SnmpService : WebApiClientBase
    {
        private readonly HttpClient _httpClinet;

        public SnmpService(HttpClient httpClinet) : base("api/snmp")
        {
            _httpClinet = httpClinet;
        }

        public async Task<MIBObject> GetAsync(ManagerUser managerUser, int rsuId, string oid)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{rsuId}/{oid}");
                var mibo = MIBObjectDto.FromJsonCollection(result);

                return MIBObject.Parse(mibo.FirstOrDefault());
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<bool> SetAsync(ManagerUser managerUser, int rsuId, MIBObject mibo)
        {
            var host = GetHost(managerUser);

            MIBObjectDto miboDto = mibo.ConvertToDTO();

            try
            {
                var result = await _httpClinet.PostAsJsonAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{rsuId}", miboDto);
                if (result.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }
    }
}
