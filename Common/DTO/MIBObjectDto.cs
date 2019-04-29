using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.DTO
{
    public partial class MIBObjectDto
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class MIBObjectDto
    {
        public static MIBObjectDto FromJson(string json) => JsonConvert.DeserializeObject<MIBObjectDto>(json, DTO.Converter.Settings);
        public static IEnumerable<MIBObjectDto> FromJsonCollection(string json) => JsonConvert.DeserializeObject<IEnumerable<MIBObjectDto>>(json, DTO.Converter.Settings);
    }
}
