using System;
using System.Collections.Generic;
using System.Text;
using SNMPManager.Core.Enumerations;

namespace SNMPManager.Core.Exceptions
{
    public class AuthorizationFailed : Exception
    {
        public AuthorizationFailed(string userName, ManagerTask task)
            : base($"Authorization failed! {userName} user have no right for task: {task.ToString()}")
        { }
    }
}
