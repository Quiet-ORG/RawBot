using Newtonsoft.Json;
using RawBot.State.Model.Items;
using System.Collections.Generic;

namespace RawBot.State.Model.Shops
{
    public class ShopItem : ItemBase
    {
        /// <summary>
        /// The shop specific item id of this item.
        /// </summary>
        [JsonProperty("ShopItemID")]
        public int ShopItemId { get; set; }

        /// <summary>
        /// The cost of the item.
        /// </summary>
        [JsonProperty("iCost")]
        public int Cost { get; set; }

        [JsonProperty("turnin")]
        public List<ItemBase> MergeRequirements { get; set; } = new();
    }
}