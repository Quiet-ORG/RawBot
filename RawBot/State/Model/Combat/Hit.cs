using Newtonsoft.Json;
using RawBot.State.Model.Entities;
using System.Collections.Generic;

namespace RawBot.State.Model.Combat
{
    public class Hit
    {
        [JsonProperty("cInf")]
        public string SourceInfo { get; set; }

        public IEntityBase Source { get; set; }

        [JsonProperty("actID")]
        public int ActionId { get; set; }

        [JsonProperty("iRes")]
        public int Res { get; set; }

        [JsonProperty("a")]
        public List<Action> Actions { get; set; } = new();
    }
}
