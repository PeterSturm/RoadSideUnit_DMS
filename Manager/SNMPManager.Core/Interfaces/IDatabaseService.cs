using System;
using System.Collections.Generic;
using System.Text;

namespace SNMPManager.Core.Interfaces
{
    public interface IDatabaseService
    {
        void AddUser();
        void RemoveUser();
        void UpdateUser();

        void AddRSU();
        void RemoveRSU();
        void UpdateRSU();
        void GetRSU();
    }
}
