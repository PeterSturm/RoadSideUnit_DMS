using SNMPManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNMPManager.WebAPI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SNMPv3Auth { get; set; }
        public string SNMPv3Priv { get; set; }

        public static User MaptoEntity(UserModel model)
        {
            return new User();
        }

        public static UserModel MapFromEntity(User user)
        {
            return new UserModel();
        }
    }
}
