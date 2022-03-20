using System;

namespace RawBot.State
{
    public class Options
    {
        public bool AggroOnRespawn { get; set; }
        public int MoveSpeed { get; set; } = 8;
        public TimeSpan GlobalCooldown { get; set; } = TimeSpan.FromSeconds(1.5d);
        public float MaxHaste { get; set; } = 0.5f;
    }
}