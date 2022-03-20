using System.Collections.Generic;

namespace RawBot.State.Model.Entities.Stats
{
    public class PlayerStats
    {
        private readonly Dictionary<string, float> _stats = new();

        public float Haste => Get("$tha");

        public float Get(string stat)
        {
            return _stats.GetValueOrDefault(stat);
        }

        public void Set(string stat, float value)
        {
            _stats[stat] = value;
        }

        public void Clear()
        {
            _stats.Clear();
        }
    }
}