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
        bool AddRSU(RSU rsu);
        bool RemoveRSU(int rsuId);
        bool UpdateRSU(RSU rsu);
        RSU GetRSU(int rsuId);
        ICollection<RSU> GetRSU();

        // User service functions
        void AddUser(User user);
        void RemoveUser(int userId);
        void UpdateUser(User user);
        User GetUser(int userId);
        ICollection<User> GetUser();
        User AuthenticateUser(string userName, string token);
        bool AuthorizeUser(string userName, string token, ManagerTask task);

        // Role services functions
        void AddRole(Role role);
        void RemoveRole(int roleId);
        void UpdateRole(Role role);
        Role GetRole(int roleId);
        ICollection<Role> GetRole();

        // Token service functions
        void AddToken(Token token);
        void RemoveToken(int tokenId);
        void UpdateToken(Token token);
        Token GetToken(int tokenId);
        ICollection<Token> GetToken();

        // SNMP manager service function


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
