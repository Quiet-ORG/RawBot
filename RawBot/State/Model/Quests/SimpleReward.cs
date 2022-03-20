using Newtonsoft.Json;

namespace RawBot.State.Model.Quests
{
    public class SimpleReward
    {
        [JsonProperty("iRate")]
        public double Rate { get; set; }

        [JsonProperty("iType")]
        public int Type { get; set; }
    }
}