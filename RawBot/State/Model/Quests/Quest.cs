using Newtonsoft.Json;
using RawBot.Converters;
using RawBot.State.Model.Items;
using System.Collections.Generic;
using System.Linq;

namespace RawBot.State.Model.Quests
{
    public class Quest
    {
        [JsonProperty("oItems")] [JsonConverter(typeof(DictionaryListConverter<int, ItemBase>))]
        private List<ItemBase> _reqs = new();

        [JsonProperty("turnin")] private List<SimpleRequirement> _turnin = new();
        private List<ItemBase> _reqCache;

        [JsonProperty("QuestID")]
        public int Id { get; set; }

        [JsonProperty("iSlot")]
        public int Slot { get; set; }

        [JsonProperty("sName")]
        public string Name { get; set; }

        [JsonProperty("sDesc")]
        public string Description { get; set; }

        [JsonProperty("sEndText")]
        public string EndText { get; set; }

        [JsonProperty("bOnce")]
        [JsonConverter(typeof(StringBoolConverter))]
        public bool Once { get; set; }

        [JsonProperty("sField")]
        public string Field { get; set; }

        [JsonProperty("iIndex")]
        public int Index { get; set; }

        [JsonProperty("iGold")]
        public int Gold { get; set; }

        [JsonProperty("iExp")]
        public int Xp { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public bool Active => Status != null;

        [JsonProperty("oReqd")]
        [JsonConverter(typeof(DictionaryListConverter<int, ItemBase>))]
        public List<ItemBase> AcceptRequirements { get; set; } = new();

        public List<ItemBase> Requirements => _reqCache ??= _reqs.Select(x => (x.Quantity = _turnin.Find(y => y.Id == x.Id)!.Quantity) != -1 ? x : x).ToList();

        [JsonProperty("oRewards")]
        [JsonConverter(typeof(QuestRewardConverter))]
        public List<ItemBase> Rewards { get; set; } = new();

        [JsonProperty("reward")]
        public List<SimpleReward> SimpleRewards { get; set; } = new();
    }
}
