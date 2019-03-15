using SNMPManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SNMPManager.Core.Exceptions
{
    public class SnmpGetError : Exception
    {

        public SnmpGetError()
        {
        }

        public SnmpGetError(string message) 
            : base(message)
        {
        }
    }
}
