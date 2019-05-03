using Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DashboardWebApp.Models
{
    public class ManagerUser
    {
        public int ManagerId { get; set; }
        public Manager Manager { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string SnmPv3Auth { get; set; }
        public string SnmPv3Priv { get; set; }
        public List<UserManagerUser> UserManagerUsers { get; set; }

        public ManagerUser()
        {

        }

        public ManagerUser(Manager manager)
        {
            Manager = manager;
            Name = "admin";
            Token = "Adminpass01";
            SnmPv3Auth = "authpass012";
            SnmPv3Priv = "privpass012";
        }

        public static ManagerUser Parse(UserDto userdto, Manager manager)
        {
            return new ManagerUser
            {
                ManagerId = manager.Id,
                Manager = manager,
                Name = userdto.UserName,
                Token = userdto.Token,
                Role = userdto.Role,
                SnmPv3Auth = userdto.SnmPv3Auth,
                SnmPv3Priv = userdto.SnmPv3Priv
            };
        }

        public UserDto ConvertToUserDto()
        {
            return new UserDto
            {
                Id = Id,
                UserName = Name,
                Token = Token,
                Role = Role,
                SnmPv3Auth = SnmPv3Auth,
                SnmPv3Priv = SnmPv3Priv
            };
        }
    }
}
