using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.DTO
{
    public partial class ManagerLogDto
    {
        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public partial class ManagerLogDto
    {
        public static ManagerLogDto FromJson(string json) => JsonConvert.DeserializeObject<ManagerLogDto>(json, DTO.Converter.Settings);
        public static IEnumerable<ManagerLogDto> FromJsonCollection(string json) => JsonConvert.DeserializeObject<IEnumerable<ManagerLogDto>>(json, DTO.Converter.Settings);
    }
}
