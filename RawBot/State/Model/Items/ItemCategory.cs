using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace RawBot.State.Model.Items
{
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemCategory
    {
        Sword,
        Axe,
        Dagger,
        Gun,
        Bow,
        Mace,
        Polearm,
        Staff,
        Wand,
        Class,
        Armor,
        Helm,
        Cape,
        Pet,
        Amulet,
        Necklace,
        Note,
        Resource,
        Item,
        [EnumMember(Value = "Quest Item")]
        QuestItem,
        [EnumMember(Value = "Server Use")]
        ServerUse,
        House,
        [EnumMember(Value = "Wall Item")]
        WallItem,
        [EnumMember(Value = "Floor Item")]
        FloorItem,
        Enhancement,
        Unknown
    }
}