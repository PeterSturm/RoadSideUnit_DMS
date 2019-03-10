using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;
using SNMPManager.Core.Exceptions;
using Microsoft.EntityFrameworkCore.Internal;
using SNMPManager.Infrastructure;

namespace SNMPManager.Persistence
{
    public class SNMPManagerService : ISNMPManagerServices 
    {
        private readonly ManagerContext _managerContext;
        private readonly ManagerLogger _logger;

        public SNMPManagerService(ManagerContext managerContext)
        {
            _managerContext = managerContext;
            _logger = new ManagerLogger(this);
        }



        #region RSU service functions
        public bool AddRSU(RSU rsu)
        {
            if (_managerContext.RSUs.Any(r => r.Id == rsu.Id))
                return false;

            _managerContext.Add(rsu);
            _managerContext.SaveChanges();

            return true;
                
        }

        public RSU GetRSU(int rsuId)
        {
            return _managerContext.RSUs.Find(rsuId);
        }

        public ICollection<RSU> GetRSU()
        {
            return _managerContext.RSUs.ToArray();
        }

        public bool RemoveRSU(int rsuId)
        {
            var rsu = _managerContext.RSUs.Find(rsuId);
            if (rsu == null)
                return false;

            _managerContext.Remove(rsu);
            _managerContext.SaveChanges();
            return true;
        }

        public bool UpdateRSU(RSU rsu)
        {
            var rsu_mod = _managerContext.RSUs.Find(rsu.Id);
            if (rsu_mod == null)
                return false;

            _managerContext.Update(rsu);
            _managerContext.SaveChanges();
            return true;
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

        public User AuthenticateUser(string userName, string token)
        {
            var user = _managerContext.Users.Find(userName);
            if (user == null)
            {
                _logger.LogAuthentication(userName, false);
                return null;
            }

            if (user.Token != token)
            {
                _logger.LogAuthentication(userName, false);
                throw new AuthenticationFailed(userName);
            }

            _logger.LogAuthentication(userName, true);
            return user;
        }

        public bool AuthorizeUser(string userName, string token, ManagerTask task)
        {
            var user = AuthenticateUser(userName, token);
            if (user == null)
            {
                _logger.LogAuthorization(userName, task, false);
                return false;
            }

            var managertasks = user.Role.ManagerTasks;
            if (managertasks == null)
            {
                _logger.LogAuthorization(userName, task, false);
                throw new AuthorizationFailed(userName, task);
            }

            if (!managertasks.Any(t => t.ToString() == task.ToString()))
            {
                _logger.LogAuthorization(userName, task, false);
                throw new AuthorizationFailed(userName, task);
            }

            _logger.LogAuthorization(userName, task, true);
            return true;
        }
        #endregion

        #region Role service functions
        public void AddRole(Role role)
        {
            throw new NotImplementedException();
        }

        public void RemoveRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public void UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Role GetRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Role> GetRole()
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
            _managerContext.Add(log);
            _managerContext.SaveChanges();
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
