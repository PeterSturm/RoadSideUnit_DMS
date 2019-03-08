using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SNMPManager.Core.Entities
{
    public class Token
    {
        public int Id { get; set; }
        public byte[] TokenValue { get; set; }
        public Role Role { get; set; }
        public bool Active { get; set; }
    }
}
