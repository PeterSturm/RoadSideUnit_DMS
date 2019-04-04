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

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{manager.Port}/api/rsu/{user.UserName}/{user.Token}");
                var rsus = RsuDto.FromJsonCollection(result);


                return rsus.Select(r => RSU.Parse(r, manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<RSU> GetAsync(Manager manager, User user, string IP, int port)
        {
            var host = (manager.IP.ToString() == "127.0.0.1") ? "localhost" : manager.IP.ToString();

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{manager.Port}/api/rsu/{user.UserName}/{user.Token}/{IP}/{port}");
                var rsu = RsuDto.FromJson(result);

                return RSU.Parse(rsu, manager);
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<RSU> GetAsync(Manager manager, User user, int id)
        {
            var host = (manager.IP.ToString() == "127.0.0.1") ? "localhost" : manager.IP.ToString();

            try
            { 
                var result = await _httpClinet.GetStringAsync($"http://{host}:{manager.Port}/api/rsu/{user.UserName}/{user.Token}/{id}");
                var rsu = RsuDto.FromJson(result);

                return RSU.Parse(rsu, manager);
            }
            catch (HttpRequestException ex)
            {
                return null;
            }

        }

        public async Task<bool> AddRSUAsync(Manager manager, User user, RSU rsu)
        {
            var host = (manager.IP.ToString() == "127.0.0.1") ? "localhost" : manager.IP.ToString();

            RsuDto rsuDto = rsu.ConvertToRSUDto();

            try
            {
                var result = await _httpClinet.PostAsJsonAsync($"http://{host}:{manager.Port}/api/rsu/{user.UserName}/{user.Token}", rsuDto);

                return true;
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateRSUAsync(Manager manager, User user, RSU rsu)
        {
            var host = (manager.IP.ToString() == "127.0.0.1") ? "localhost" : manager.IP.ToString();

            try
            {
                var result = await _httpClinet.PutAsJsonAsync($"http://{host}:{manager.Port}/api/rsu/{user.UserName}/{user.Token}", rsu.ConvertToRSUDto());
                return true;
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRSUAsync(Manager manager, User user, int rsuId)
        {
            var host = (manager.IP.ToString() == "127.0.0.1") ? "localhost" : manager.IP.ToString();

            try
            {
                var result = await _httpClinet.DeleteAsync($"http://{host}:{manager.Port}/api/rsu/{user.UserName}/{user.Token}/{rsuId}");
                return true;
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }
    }
}
