using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;
using System;
using System.Net;

namespace SNMPManager.Infrastructure
{
    

    public class SNMPManagerService : ISNMPManagerService
    {
        public MIBObject Get(RSU rsu, User user, string OID)
        {
            throw new NotImplementedException();
        }

        public bool Set(RSU rsu, User user, string OID, MIBObjectType type, object value)
        {
            throw new NotImplementedException();
        }

        public void AddSNMPUser(RSU rsu, User user)
        {
            throw new NotImplementedException();
        }

        public void SetTrapListener(RSU rsu, IPAddress listenerIP, int listenerPort)
        {
            throw new NotImplementedException();
        }
    }
}
