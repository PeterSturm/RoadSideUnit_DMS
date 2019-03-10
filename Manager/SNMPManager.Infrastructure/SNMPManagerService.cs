using SNMPManager.Core.Entities;
using SNMPManager.Core.Interfaces;
using System;
using System.Net;

namespace SNMPManager.Infrastructure
{
    public class SNMPManagerService : ISNMPManagerService
    {
        public void Get(RSU rsu, User user, string OID)
        {
            throw new NotImplementedException();
        }

        public void Set(RSU rsu, User user, string OID, object value)
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
