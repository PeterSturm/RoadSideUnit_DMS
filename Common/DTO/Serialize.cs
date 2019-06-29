using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO
{
    public static class Serialize
    {
        public static string ToJson(this UserDto self) => JsonConvert.SerializeObject(self, DTO.Converter.Settings);
        public static string ToJson(this RsuDto self) => JsonConvert.SerializeObject(self, DTO.Converter.Settings);
        public static string ToJson(this ManagerLogDto self) => JsonConvert.SerializeObject(self, DTO.Converter.Settings);
        public static string ToJson(this MIBObjectDto self) => JsonConvert.SerializeObject(self, DTO.Converter.Settings);
        public static string ToJson(this TrapLogDto self) => JsonConvert.SerializeObject(self, DTO.Converter.Settings);
    }
}
