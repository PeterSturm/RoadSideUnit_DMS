using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SNMPManager.Core.Interfaces
{
    public interface ILogger
    {
        void Log(LogType level, string message);
        void LogAuthentication(string userName, bool success);
        void LogAuthorization(string username, bool success);
        void LogDBOperation(string userName, DBOperation Operation);
        void LogAPICall(string userName, ManagerOperation managerOperation);
    }
}
