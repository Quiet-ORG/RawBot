using Newtonsoft.Json;
using RawBot.Converters;
using System.Collections.Generic;

namespace RawBot.State.Model.Entities
{
    public class Monster : EntityBase, IMonster
    {
        [JsonProperty("MonMapID")]
        public int MapId { get; set; }

        [JsonProperty("MonID")]
        public int DefinitionId { get; set; }

        [JsonProperty("strMonName")]
        private string MonsterName
        {
            get => Name;
            set => Name = value;
        }

        [JsonProperty("bRed")]
        [JsonConverter(typeof(StringBoolConverter))]
        public bool AutoAggro { get; set; }

        [JsonProperty("strMonFileName")]
        public string FileName { get; set; }

        [JsonProperty("strLinkage")]
        public string Linkage { get; set; }

        [JsonProperty("strElement")]
        public string Element { get; set; }

        [JsonProperty("strBehave")]
        public string Behaviour { get; set; }

        [JsonProperty("strRace")]
        public string Race { get; set; }

        [JsonProperty("targets")]
        public List<string> Targets { get; set; } = new();

        public override string TargetString => $"m:{MapId}";
        public override int TargetId => MapId;

        public bool IsAttacking(Player player)
        {
            return Targets.Contains(player.Username);
        }

        protected override void OnDeath()
        {
            Targets.Clear();

            base.OnDeath();
        }
    }
    
    public interface IMonster : IEntityBase, IState
    {
        int MapId { get; set; }
        int DefinitionId { get; set; }
        bool AutoAggro { get; set; }
        string FileName { get; set; }
        string Linkage { get; set; }
        string Element { get; set; }
        string Behaviour { get; set; }
        string Race { get; set; }
        List<string> Targets { get; set; }

        bool IsAttacking(Player player);
    }
}
