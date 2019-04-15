using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DashboardWebApp.Models;
using Common.DTO;

namespace DashboardWebApp.WebApiClients
{
    public class UserService : WebApiClientBase
    {
        private readonly HttpClient _httpClinet;

        public UserService(HttpClient httpClinet) : base("api/user")
        {
            _httpClinet = httpClinet;
        }

        public async Task<IEnumerable<ManagerUser>> GetAsync(ManagerUser managerUser)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}");
                var users = UserDto.FromJsonCollection(result);


                return users.Select(u => ManagerUser.Parse(u, managerUser.Manager)).ToList();
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<ManagerUser> GetAsync(ManagerUser managerUser, int id)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.GetStringAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{id}");
                UserDto user = UserDto.FromJson(result);


                return ManagerUser.Parse(user, managerUser.Manager);
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        public async Task<bool> AddAsync(ManagerUser managerUser, ManagerUser newManagerUser)
        {
            var host = GetHost(managerUser);

            UserDto userDto = newManagerUser.ConvertToUserDto();

            try
            {
                var result = await _httpClinet.PostAsJsonAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}", userDto);
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

        public async Task<bool> UpdateAsync(ManagerUser managerUser, ManagerUser modManagerUser)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.PutAsJsonAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}", modManagerUser.ConvertToUserDto());
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

        public async Task<bool> DeleteAsync(ManagerUser managerUser, int managerUserId)
        {
            var host = GetHost(managerUser);

            try
            {
                var result = await _httpClinet.DeleteAsync($"http://{host}:{managerUser.Manager.Port}/{controller}/{managerUser.Name}/{managerUser.Token}/{managerUserId}");
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
