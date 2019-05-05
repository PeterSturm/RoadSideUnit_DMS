using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.Models
{
    public class MIBObject
    {
        public string OID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public static MIBObject Parse(MIBObjectDto mibObjectDto)
        {
            if(mibObjectDto != null)
                return new MIBObject
                    {
                        OID = mibObjectDto.Oid,
                        Type = mibObjectDto.Type,
                        Value = mibObjectDto.Value
                    };

            return new MIBObject
                {
                    OID = "",
                    Type = "",
                    Value = ""
                };

        }

        public MIBObjectDto ConvertToDTO()
        {
            return new MIBObjectDto
            {
                Oid = OID,
                Type = Type,
                Value = Value
            };
        }
    }
}
