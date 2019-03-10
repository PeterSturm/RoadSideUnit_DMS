using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SNMPManager.Core.Enumerations;

namespace SNMPManager.Core.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public ICollection<ManagerTask> ManagerTasks { get; set; }
    }
}
