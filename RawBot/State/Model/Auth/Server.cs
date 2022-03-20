using Newtonsoft.Json;

namespace RawBot.State.Model.Auth
{
    public class Server
    {
        [JsonProperty("sName")]
        public string Name { get; set; }

        [JsonProperty("sIP")]
        public string Ip { get; set; }

        [JsonProperty("iCount")]
        public int Players { get; set; }

        [JsonProperty("iMax")]
        public int MaxPlayers { get; set; }

        [JsonProperty("bOnline")]
        public bool Online { get; set; }

        [JsonProperty("iChat")]
        public int ChatMode { get; set; }

        [JsonProperty("bUpg")]
        public bool Upgrade { get; set; }

        [JsonProperty("sLang")]
        public string Language { get; set; }

        [JsonProperty("iPort")]
        public ushort Port { get; set; }
    }
}
