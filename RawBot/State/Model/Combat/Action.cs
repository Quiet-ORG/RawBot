using Newtonsoft.Json;
using RawBot.State.Model.Entities;

namespace RawBot.State.Model.Combat
{
    public class Action
    {
        [JsonProperty("hp")]
        public int Damage { get; set; }

        [JsonProperty("tInf")]
        public string TargetInfo { get; set; }

        public IEntityBase Target { get; set; }

        [JsonProperty("type")]
        public ActionType Type { get; set; }
    }
}
