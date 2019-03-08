using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SNMPManager.Core.Interfaces;

namespace SNMPManager.Persistence
{
    public class DBService : IDatabaseService 
    {
        private readonly ManagerContext managerContext;

        public DBService(ManagerContext managerContext)
        {
            this.managerContext = managerContext;
        }
    }
}
