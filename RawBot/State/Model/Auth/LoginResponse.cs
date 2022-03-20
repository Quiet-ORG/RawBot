using Newtonsoft.Json;
using System.Collections.Generic;

namespace RawBot.State.Model.Auth
{
    public class LoginResponse
    {
        [JsonProperty("login")]
        public UserInfo User { get; set; }

        [JsonProperty("servers")]
        public List<Server> Servers { get; set; } = new();
    }
}