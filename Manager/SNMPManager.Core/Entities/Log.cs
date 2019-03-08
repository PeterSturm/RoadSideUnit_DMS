using SNMPManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SNMPManager.Core.Entities
{
    [NotMapped]
    public abstract class Log
    {
        public DateTime TimeStamp { get; set; }
        public LogLevel Type { get; set; }
        public string Message { get; set; }
    }
}
