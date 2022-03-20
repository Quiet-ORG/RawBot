using Newtonsoft.Json;
using RawBot.Converters;

namespace RawBot.State.Model.Items
{
    public class ItemBase
    {
        [JsonProperty("ItemID")]
        public virtual int Id { get; set; }

        [JsonProperty("sName")]
        [JsonConverter(typeof(TrimConverter))]
        public virtual string Name { get; set; }

        [JsonProperty("sDesc")]
        public virtual string Description { get; set; }

        [JsonProperty("iQty")]
        public virtual int Quantity { get; set; }

        [JsonProperty("iStk")]
        public virtual int MaxStack { get; set; }

        [JsonProperty("bUpg")]
        [JsonConverter(typeof(StringBoolConverter))]
        public virtual bool Upgrade { get; set; }

        [JsonProperty("bCoins")]
        [JsonConverter(typeof(StringBoolConverter))]
        public virtual bool Coins { get; set; }

        [JsonProperty("sType")]
        public virtual ItemCategory Category { get; set; }

        [JsonProperty("bTemp")]
        [JsonConverter(typeof(StringBoolConverter))]
        public virtual bool Temp { get; set; }

        [JsonProperty("sFaction")]
        public virtual string Faction { get; set; }
    }
}