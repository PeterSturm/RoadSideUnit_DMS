using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(Level level, string message)
        {
            Console.WriteLine($"Loglevel: {level.ToString()} - {message}");
        }
    }
}
