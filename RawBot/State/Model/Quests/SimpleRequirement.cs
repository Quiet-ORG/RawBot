using Newtonsoft.Json;

namespace RawBot.State.Model.Quests
{
    public class SimpleRequirement
    {
        [JsonProperty("ItemID")]
        public int Id { get; set; }

        [JsonProperty("iQty")]
        public int Quantity { get; set; }
    }
}