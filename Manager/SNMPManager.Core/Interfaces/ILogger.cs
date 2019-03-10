using SNMPManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SNMPManager.Core.Interfaces
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
        void LogAuthentication(string userName, bool success);
        void LogAuthorization(string username, ManagerTask task, bool success);
        void LogDBOperation(string userName, Operation Operation);
        void LogAPICall(string userName, ManagerTask managerTask, Operation Operation);
    }
}
