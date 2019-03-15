using SNMPManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using SNMPManager.Core.Enumerations;

namespace SNMPManager.Core.Interfaces
{
    public interface ISNMPManagerService
    {
        MIBObject Get(RSU rsu, User user, string OID);
        bool Set(RSU rsu, User user, string OID, MIBObjectType type, object value);

        void SetTrapListener(RSU rsu, IPAddress listenerIP, int listenerPort);
        void AddSNMPUser(RSU rsu, User user);
    }
}
