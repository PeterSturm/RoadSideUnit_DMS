using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SNMPManager.Core.Entities
{
    public class TrapLog
    {
        [Key]
        public DateTime TimeStamp { get; set; }
        [Key]
        public int SourceRSU { get; set; }
        [Key]
        public int TypeId { get; set; }
        public LogType Type { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
