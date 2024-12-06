using Newtonsoft.Json;
using RawBot.Network;
using RawBot.Runtime;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RawBot.State.Model.Entities
{
    public class Player : EntityBase, IPlayer
    {
        public const int LevelCap = 100;

        private Game game => new Game();

        [JsonProperty("entID")]
        public int Id { get; set; }

        [JsonProperty("uoName")]
        public string Username { get; set; }

        [JsonProperty("afk")]
        public bool Afk { get; set; }

        public Dictionary<string, Equipment> Equipment { get; set; } = new();

        [JsonProperty("strClassName")]
        public string ClassName { get; set; }

        [JsonProperty("iCP")]
        public int ClassPoints { get; set; }

        [JsonProperty("strGender")]
        public string Gender { get; set; }

        [JsonProperty("iBagSlots")]
        public int BagSlots { get; set; }

        [JsonProperty("iBankSlots")]
        public int BankSlots { get; set; }

        public int BankSlotsUsed { get; set; }

        [JsonProperty("iHouseSlots")]
        public int HouseSlots { get; set; }

        [JsonProperty("intExp")]
        public int Xp { get; set; }

        [JsonProperty("intExpToLevel")]
        public int RequiredXp { get; set; }

        [JsonProperty("intGold")]
        public int Gold { get; set; }

        [JsonProperty("intCoins")]
        public int Coins { get; set; }

        [JsonProperty("wDPS")]
        public float Dps { get; set; }

        public override string TargetString => $"p:{Id}";
        public override int TargetId => Id;
    }
    
    public interface IPlayer : IEntityBase, IState
    {
        int Id { get; set; }
        string Username { get; set; }
        bool Afk { get; set; }
        Dictionary<string, Equipment> Equipment { get; set; }
        string ClassName { get; set; }
        int ClassPoints { get; set; }
        string Gender { get; set; }
        int BagSlots { get; set; }
        int BankSlots { get; set; }
        int BankSlotsUsed { get; set; }
        int HouseSlots { get; set; }
        int Xp { get; set; }
        int RequiredXp { get; set; }
        int Gold { get; set; }
        int Coins { get; set; }
        float Dps { get; set; }
    }
}
