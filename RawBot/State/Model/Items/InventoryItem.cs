using Newtonsoft.Json;
using RawBot.Converters;

namespace RawBot.State.Model.Items
{
    public class InventoryItem : ItemBase
    {
        [JsonProperty("CharItemID")]
        public int CharItemId { get; set; }

        [JsonProperty("bEquip")]
        [JsonConverter(typeof(StringBoolConverter))]
        public bool Equipped { get; set; }

        [JsonProperty("sMeta")]
        public string Meta { get; set; }

        [JsonProperty("iLvl")]
        public int Level { get; set; }
    }
}