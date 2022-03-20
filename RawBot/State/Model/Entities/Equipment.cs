using Newtonsoft.Json;

namespace RawBot.State.Model.Entities
{
    public class Equipment
    {
        [JsonProperty("ItemID")]
        public int ItemId { get; set; }
        [JsonProperty("sFile")]
        public string FileName { get; set; }
        [JsonProperty("sLink")]
        public string Linkage { get; set; }
    }
}