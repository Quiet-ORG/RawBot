using Newtonsoft.Json;
using System;

namespace RawBot.State.Model.Combat.Skills
{
    public class Skill
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nam")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("mp")]
        public int Mp { get; set; }

        [JsonProperty("anim")]
        public string Animation { get; set; }

        public string[] Animations => Animation?.Split(',') ?? Array.Empty<string>();

        [JsonProperty("typ")]
        public string Type { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("isOK")]
        public bool Ok { get; set; }

        [JsonProperty("auto")]
        public bool Auto { get; set; }

        [JsonProperty("range")]
        public int Range { get; set; }

        [JsonProperty("cd")]
        public int Cooldown { get; set; }

        public TimeSpan CooldownSpan => TimeSpan.FromMilliseconds(Cooldown);

        [JsonProperty("damage")]
        public float Damage { get; set; }

        [JsonProperty("tgt")]
        public string Target { get; set; }

        [JsonProperty("tgtMin")]
        public int MinTargets { get; set; }

        private int _maxTargets;

        [JsonProperty("tgtMax")]
        public int MaxTargets
        {
            get => Math.Max(1, _maxTargets);
            set => _maxTargets = value;
        }

        public int Index { get; set; }

        public TimeSpan LastUse { get; set; }
    }
}
