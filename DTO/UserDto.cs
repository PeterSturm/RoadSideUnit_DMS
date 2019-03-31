using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DTO
{
    public partial class UserDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("snmPv3Auth")]
        public string SnmPv3Auth { get; set; }

        [JsonProperty("snmPv3Priv")]
        public string SnmPv3Priv { get; set; }
    }

    public partial class UserDto
    {
        public static UserDto FromJson(string json) => JsonConvert.DeserializeObject<UserDto>(json, DTO.Converter.Settings);
    }
}

