using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;

namespace SNMPManager.Persistence
{
    public class SNMPManagerService : ISNMPManagerServices 
    {
        private readonly ManagerContext managerContext;

        public SNMPManagerService(ManagerContext managerContext)
        {
            this.managerContext = managerContext;
        }



        #region RSU service functions
        public void AddRSU(RSU rsu)
        {
            throw new NotImplementedException();
        }

        public RSU GetRSU(int rsuId)
        {
            throw new NotImplementedException();
        }

        public ICollection<RSU> GetRSU()
        {
            throw new NotImplementedException();
        }

        public void RemoveRSU(int rsuId)
        {
            throw new NotImplementedException();
        }

        public void UpdateRSU(RSU rsu)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region User service functions
        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetUser()
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(int userId)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Token service functions
        public void AddToken(Token token)
        {
            throw new NotImplementedException();
        }

        public Token GetToken(int tokenId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Token> GetToken()
        {
            throw new NotImplementedException();
        }

        public void UpdateToken(Token token)
        {
            throw new NotImplementedException();
        }

        public void RemoveToken(int tokenId)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Log service functions
        public void AddManagerLog(ManagerLog log)
        {
            throw new NotImplementedException();
        }

        public void AddTrapLog(TrapLog log)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ManagerLog> GetManagerLogs()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ManagerLog> GetManagerLogs(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ManagerLog> GetManagerLogs(LogLevel logLevel, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrapLog> GetTrapLogs()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrapLog> GetTrapLogs(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrapLog> GetTrapLogs(LogLevel logLevel, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrapLog> GetTrapLogs(int rsuId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrapLog> GetTrapLogs(int rsuId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
