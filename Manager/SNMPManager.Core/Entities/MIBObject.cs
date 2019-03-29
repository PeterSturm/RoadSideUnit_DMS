using SNMPManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Lextm.SharpSnmpLib;

namespace SNMPManager.Core.Entities
{
    [NotMapped]
    public class MIBObject
    {
        public string OID { get; set; }
        private SnmpType _type;
        public string Type { get { return _type.ToString(); } }
        public string Value { get; set; }

        public MIBObject(string oid, SnmpType type, string value)
        {
            OID = oid;
            _type = type;
            Value = value;
        }
    }
}
