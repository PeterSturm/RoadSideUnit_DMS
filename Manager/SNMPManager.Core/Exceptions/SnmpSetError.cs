using SNMPManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SNMPManager.Core.Exceptions
{
    public class SnmpSetError : Exception
    {

        public SnmpSetError()
        {
        }

        public SnmpSetError(string message) 
            : base(message)
        {
        }
    }
}
