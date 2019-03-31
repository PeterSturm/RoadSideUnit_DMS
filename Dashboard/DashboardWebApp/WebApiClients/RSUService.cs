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
    public class RSUService : IRSUService
    {
        private readonly HttpClient _httpClinet;

        public RSUService(HttpClient httpClinet)
        {
            _httpClinet = httpClinet;
        }

        public async Task<IEnumerable<RSU>> GetAsync(Manager manager, User user)
        {
            var host = (manager.IP.ToString() == "127.0.0.1") ? "localhost" : manager.IP.ToString();

            var result = await _httpClinet.GetStringAsync($"http://{host}:{manager.Port}/api/rsu/{user.UserName}/{user.Token}");
            var rsus = RsuDto.FromJsonCollection(result);

            return rsus.Select(r => new RSU
            {
                Id = r.Id,
                Name = r.Name,
                IP = IPAddress.Parse(r.Ip),
                Port = r.Port,
                Manager = manager
            }).ToList();
        }

        public async Task<RSU> GetAsync(Manager manager, User user, string IP, int port)
        {
            var result = await _httpClinet.GetStringAsync($"http://{manager.IP}:{manager.Port}/api/rsu/?username={user.UserName}&token={user.Token}&ip={IP}&port={port}");
            var rsu = RsuDto.FromJson(result);

            return new RSU
            {
                Id = rsu.Id,
                Name = rsu.Name,
                IP = IPAddress.Parse(rsu.Ip),
                Port = rsu.Port,
                Manager = manager
            };
        }

        public async Task<RSU> GetAsync(Manager manager, User user, int id)
        {
            var result = await _httpClinet.GetStringAsync($"http://{manager.IP}:{manager.Port}/api/rsu/?username={user.UserName}&token={user.Token}&id={id}");
            var rsu = RsuDto.FromJson(result);

            return new RSU
            {
                Id = rsu.Id,
                Name = rsu.Name,
                IP = IPAddress.Parse(rsu.Ip),
                Port = rsu.Port,
                Manager = manager
            };
        }

        public async Task AddRSUAsync(Manager manager, User user, RSU rsu)
        {
            //var result = await _httpClinet.GetStringAsync($"http://{manager.IP}:{manager.Port}/api/rsu/?username={user.UserName}&token={user.Token}&ip={IP}&port={port}");
            var result = await _httpClinet.PostAsJsonAsync($"http://{manager.IP}:{manager.Port}/api/rsu/?username={user.UserName}&token={user.Token}", rsu);
            /*return new RSU
            {
                Id = rsu.Id,
                Name = rsu.Name,
                IP = IPAddress.Parse(rsu.Ip),
                Port = rsu.Port,
                Manager = manager
            };*/
        }

        public async Task UpdateRSUAsync(Manager manager, User user, RSU rsu)
        {
            var result = await _httpClinet.PutAsJsonAsync($"http://{manager.IP}:{manager.Port}/api/rsu/?username={user.UserName}&token={user.Token}", rsu);
        }

        public async Task DeleteRSUAsync(Manager manager, User user, int rsuId)
        {
            var result = await _httpClinet.DeleteAsync($"http://{manager.IP}:{manager.Port}/api/rsu/?username={user.UserName}&token={user.Token}&id={rsuId}");
        }
    }
}
