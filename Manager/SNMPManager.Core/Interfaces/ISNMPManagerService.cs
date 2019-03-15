using SNMPManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using SNMPManager.Core.Enumerations;
using Lextm.SharpSnmpLib;

namespace SNMPManager.Core.Interfaces
{
    public interface ISNMPManagerService
    {
        void Configure(ManagerSettings settings);

        IList<MIBObject> Get(RSU rsu, User user, string OID);
        bool Set(RSU rsu, User user, string OID, SnmpType type, object value);

        void SetTrapListener(RSU rsu, IPAddress listenerIP, int listenerPort);
    }
}
