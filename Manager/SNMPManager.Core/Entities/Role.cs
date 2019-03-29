using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SNMPManager.Core.Enumerations;

namespace SNMPManager.Core.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
