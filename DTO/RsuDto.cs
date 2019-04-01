using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DTO
{
    public partial class RsuDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("mibVersion")]
        public string MIBVersion { get; set; }

        [JsonProperty("firmwareVersion")]
        public string FirmwareVersion { get; set; }

        [JsonProperty("locationDescription")]
        public string LocationDescription { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("notificationIP")]
        public string NotificationIP { get; set; }

        [JsonProperty("notificationPort")]
        public int NotificationPort { get; set; }
    }

    public partial class RsuDto
    {
        public static RsuDto FromJson(string json) => JsonConvert.DeserializeObject<RsuDto>(json, DTO.Converter.Settings);
        public static IEnumerable<RsuDto> FromJsonCollection(string json) => JsonConvert.DeserializeObject<IEnumerable<RsuDto>>(json, DTO.Converter.Settings);
    }
}

