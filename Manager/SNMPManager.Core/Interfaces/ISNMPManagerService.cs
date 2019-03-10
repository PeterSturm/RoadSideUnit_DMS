using SNMPManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace SNMPManager.Core.Interfaces
{
    public interface ISNMPManagerService
    {
        // TO BE FIXED params propably won't be the best
        void Get(RSU rsu, User user, string OID);
        void Set(RSU rsu, User user, string OID, object value);

        void SetTrapListener(RSU rsu, IPAddress listenerIP, int listenerPort);
        void AddSNMPUser(RSU rsu, User user);
    }
}
