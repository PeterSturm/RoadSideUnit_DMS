using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;

namespace SNMPManager.Infrastructure
{
    public class ManagerLogger : ILogger
    {
        private ISNMPManagerServices _SNMPManagerServices;

        public ManagerLogger(ISNMPManagerServices SNMPManagerServices)
        {
            _SNMPManagerServices = SNMPManagerServices;
        }
        public void Log(LogLevel level, string message)
        {
            ManagerLog log = new ManagerLog
            {
                TimeStamp = DateTime.UtcNow,
                Type = level,
                Message = message
            };

            _SNMPManagerServices.AddManagerLog(log);
        }

        public void LogAPICall(string userName, ManagerTask managerTask, Operation Operation)
        {
            if (managerTask == ManagerTask.ADMINISTRATION)
                Log(LogLevel.APICALL, $"{userName} requested {managerTask.ToString()} task with operation {Operation.ToString()}.");
            else
                Log(LogLevel.APICALL, $"{userName} requested {managerTask.ToString()} task.");
        }

        public void LogAuthentication(string userName, bool success)
        {
            Log(LogLevel.SECURITY, $"Authentication for {userName} was {GetSuccessString(success)}!");
        }

        public void LogAuthorization(string userName, ManagerTask task, bool success)
        {
            Log(LogLevel.SECURITY, $"Authorization for {userName} was {GetSuccessString(success)} because missing rights to {task.ToString()}!");
        }

        public void LogDBOperation(string userName, Operation Operation)
        {
            Log(LogLevel.DB, $"{Operation.ToString()} operation executed for {userName}.");
        }

        private string GetSuccessString(bool success)
        {
            return success ? "successful!" : "unsuccessful!";
        }
    }
}
