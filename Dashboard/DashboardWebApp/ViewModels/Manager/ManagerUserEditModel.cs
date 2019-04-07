using DashboardWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewModels.Manager
{
    public enum Status { UNCHANGED, CREATED, DELETED, MODIFIED }

    public class ManagerUserEditModel : ManagerUser
    {
        public Status Status { get; set; } = Status.UNCHANGED;

        public ManagerUserEditModel()
        {

        }

        public ManagerUserEditModel(ManagerUser managerUser)
        {
            ManagerId = managerUser.ManagerId;
            Manager = managerUser.Manager;
            Id = managerUser.Id;
            Name = managerUser.Name;
            Token = managerUser.Token;
            Role = managerUser.Role;
            SnmPv3Auth = managerUser.SnmPv3Auth;
            SnmPv3Priv = managerUser.SnmPv3Priv;
            UserManagerUsers = managerUser.UserManagerUsers;
        }
    }
}
