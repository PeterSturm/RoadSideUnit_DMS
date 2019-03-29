using SNMPManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SNMPManager.Core.Exceptions
{
    public class AuthorizationFailed : Exception
    {
        public AuthorizationFailed(string userName)
            : base($"Authorization failed! {userName} user have no right for this task!")
        { }
    }
}
