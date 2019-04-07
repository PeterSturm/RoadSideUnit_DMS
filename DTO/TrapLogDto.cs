using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DTO
{
    public partial class TrapLogDto
    {
        [JsonProperty("sourceRSU")]
        public int SourceRsu { get; set; }

        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public partial class TrapLogDto
    {
        public static TrapLogDto FromJson(string json) => JsonConvert.DeserializeObject<TrapLogDto>(json, DTO.Converter.Settings);
        public static IEnumerable<TrapLogDto> FromJsonCollection(string json) => JsonConvert.DeserializeObject<IEnumerable<TrapLogDto>>(json, DTO.Converter.Settings);
    }
}
