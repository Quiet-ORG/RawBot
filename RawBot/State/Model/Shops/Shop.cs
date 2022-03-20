using Newtonsoft.Json;
using RawBot.State.Model.Items;
using System.Linq;

namespace RawBot.State.Model.Shops
{
    public class Shop :IShop
    {
        [JsonProperty("ShopID")]
        public int Id { get; set; }

        [JsonProperty("sName")]
        public string Name { get; set; }

        [JsonProperty("bUpgrd")]
        public bool Upgrade { get; set; }

        [JsonProperty("items")]
        public Inventory<ShopItem> Items { get; } = new();

        public bool Merge => Items.Any(i => i.MergeRequirements.Count > 0);

        public bool Loaded => Name is not null;
    }

    public interface IShop : IState
    {
        int Id { get; set; }
        string Name { get; set; }
        bool Upgrade { get; set; }
        public Inventory<ShopItem> Items { get; }
        public bool Merge { get; }
        public bool Loaded { get; }
    }
}
