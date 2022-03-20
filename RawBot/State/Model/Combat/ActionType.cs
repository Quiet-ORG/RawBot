using System.Runtime.Serialization;

namespace RawBot.State.Model.Combat
{
    public enum ActionType
    {
        [EnumMember(Value = "hit")] Hit,
        [EnumMember(Value = "crit")] Crit,
        [EnumMember(Value = "dodge")] Dodge,
        [EnumMember(Value = "miss")] Miss
    }
}
