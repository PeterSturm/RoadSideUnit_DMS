using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNMPManager.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public Role Role { get; set; }
        public string SNMPv3Auth { get; set; }
        public string SNMPv3Priv { get; set; }
    }
}
