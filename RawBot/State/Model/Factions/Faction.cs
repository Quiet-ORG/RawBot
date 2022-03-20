using Newtonsoft.Json;

namespace RawBot.State.Model.Factions
{
    public class Faction
    {
        [JsonProperty("FactionID")]
        public int Id { get; set; }

        [JsonProperty("sName")]
        public string Name { get; set; }

        [JsonProperty("iRep")]
        public int Reputation { get; set; }

        [JsonProperty("CharFactionID")]
        public long CharFactionId { get; set; }
    }
}