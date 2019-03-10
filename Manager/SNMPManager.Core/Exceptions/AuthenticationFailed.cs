using System;
using System.Collections.Generic;
using System.Text;

namespace SNMPManager.Core.Exceptions
{
    public class AuthenticationFailed : Exception
    {
        public AuthenticationFailed(string userName)
            : base($"Authentication for {userName} user faild. Wrong token!")
        { }
    }
}
