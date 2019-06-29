using System;
using System.Collections.Generic;
using System.Text;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;

namespace SNMPManager.Core.Interfaces
{
    public interface IContextService
    {
        // RSU service functions
        bool AddRSU(RSU rsu);
        bool RemoveRSU(int rsuId);
        bool UpdateRSU(RSU rsu);
        RSU GetRSU(int rsuId);
        ICollection<RSU> GetRSU();

        // User service functions
        bool AddUser(User user);
        bool RemoveUser(int userId);
        bool UpdateUser(User user);
        User GetUser(int userId);
        User GetUser(string userName);
        ICollection<User> GetUser();
        User AuthenticateUser(string userName, string token);
        bool AuthorizeUser(string userName, string token);

        // Role services functions
        bool AddRole(Role role);
        bool RemoveRole(int roleId);
        bool UpdateRole(Role role);
        Role GetRole(int roleId);
        Role GetRole(string name);
        ICollection<Role> GetRole();

        // SNMP manager settings service function
        bool AddManagerSettings(ManagerSettings settings);
        bool UpdateManagerSettings(ManagerSettings settings);

        // Log service functions
        void AddManagerLog(ManagerLog log);
        void AddTrapLog(TrapLog log);
        ManagerSettings GetManagerSettings();

        IEnumerable<ManagerLog> GetManagerLogs();
        IEnumerable<TrapLog> GetTrapLogs();
        IEnumerable<ManagerLog> GetManagerLogs(DateTime from, DateTime to);
        IEnumerable<TrapLog> GetTrapLogs(DateTime from, DateTime to);
        IEnumerable<ManagerLog> GetManagerLogs(LogType logLevel, DateTime from, DateTime to);
        IEnumerable<TrapLog> GetTrapLogs(LogType logLevel, DateTime from, DateTime to);
        IEnumerable<TrapLog> GetTrapLogs(int rsuId, DateTime from, DateTime to);
        IEnumerable<TrapLog> GetTrapLogs(int rsuId);

    }
}
