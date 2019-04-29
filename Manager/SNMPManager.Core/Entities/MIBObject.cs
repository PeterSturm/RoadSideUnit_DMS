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
        //private SnmpType _type;
        //public string Type { get { return _type.ToString(); } }
        public SnmpType Type { get; set; }
        public string Value { get; set; }

        public MIBObject(string oid, SnmpType type, string value)
        {
            OID = oid;
            //_type = type;
            Type = type;
            Value = value;
        }

        public MIBObject(string oid, string type, string value)
        {
            OID = oid;

            SnmpType temp;
            if (Enum.TryParse(type, out temp))
                Type = temp;
            else
                Type = SnmpType.Unknown;

            /*switch (Type)
            {
                case SnmpType.Integer32:
                    Value = new Integer32(int.Parse(value));
                    break;
                case SnmpType.OctetString:
                    Value = new OctetString(value);
                    break;
                case SnmpType.Counter32:
                    Value = new Counter32(int.Parse(value));
                    break;
                case SnmpType.Gauge32:
                    Value = new Gauge32(int.Parse(value));
                    break;
                case SnmpType.TimeTicks:
                    Value = new TimeTicks(uint.Parse(value));
                    break;
                case SnmpType.Counter64:
                    Value = new Counter64(ulong.Parse(value));
                    break;
                default:
                    Value = value;
                    break;
            }*/

            Value = value;
        }

    }
}
