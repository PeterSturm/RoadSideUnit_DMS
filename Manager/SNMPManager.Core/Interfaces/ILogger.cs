using System;
using System.Collections.Generic;
using System.Text;

namespace SNMPManager.Core.Interfaces
{
    public enum Level {ADMIN, MANAGER, DB};

    public interface ILogger
    {
        void Log(Level level, string message);
    }
}
