using System;
using System.Collections.Generic;
using System.Text;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;

namespace SNMPManager.Core.Interfaces
{
    public interface ISNMPManagerServices
    {
        // RSU service functions
        void AddRSU(RSU rsu);
        void RemoveRSU(int rsuId);
        void UpdateRSU(RSU rsu);
        RSU GetRSU(int rsuId);
        ICollection<RSU> GetRSU();

        // User service functions
        void AddUser(User user);
        void RemoveUser(int userId);
        void UpdateUser(User user);
        User GetUser(int userId);
        ICollection<User> GetUser();

        // Token service functions
        void AddToken(Token token);
        void RemoveToken(int tokenId);
        void UpdateToken(Token token);
        Token GetToken(int tokenId);
        ICollection<Token> GetToken();

        // Log service functions
        void AddManagerLog(ManagerLog log);
        void AddTrapLog(TrapLog log);

        IEnumerable<ManagerLog> GetManagerLogs();
        IEnumerable<TrapLog> GetTrapLogs();
        IEnumerable<ManagerLog> GetManagerLogs(DateTime from, DateTime to);
        IEnumerable<TrapLog> GetTrapLogs(DateTime from, DateTime to);
        IEnumerable<ManagerLog> GetManagerLogs(LogLevel logLevel, DateTime from, DateTime to);
        IEnumerable<TrapLog> GetTrapLogs(LogLevel logLevel, DateTime from, DateTime to);
        IEnumerable<TrapLog> GetTrapLogs(int rsuId, DateTime from, DateTime to);
        IEnumerable<TrapLog> GetTrapLogs(int rsuId);

    }
}
