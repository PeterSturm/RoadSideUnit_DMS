using SNMPManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SNMPManager.Core.Entities
{
    [NotMapped]
    public class MIBObject
    {
        public string OID { get; set; }
        public MIBObjectType Type { get; set; }
        public object Value { get; set; }
    }
}
