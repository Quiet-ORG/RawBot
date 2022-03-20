using Newtonsoft.Json;

namespace RawBot.State.Model.Entities
{
    public delegate void EntityDeathHandler(EntityBase entity);

    public abstract class EntityBase : IEntityBase
    {
        private EntityState _state;

        [JsonProperty("strUsername")]
        public string Name { get; set; }

        [JsonProperty("intHP")]
        public int Health { get; set; }

        [JsonProperty("intHPMax")]
        public int MaxHealth { get; set; }

        [JsonProperty("intMP")]
        public int Mana { get; set; }

        [JsonProperty("intMPMax")]
        public int MaxMana { get; set; }

        [JsonProperty("iLvl")]
        public int Level { get; set; }

        [JsonProperty("intLevel")]
        private int Level2
        {
            get => Level;
            set => Level = value;
        }

        [JsonProperty("strFrame")]
        public string Frame { get; set; }

        [JsonProperty("strPad")]
        public string Pad { get; set; }

        [JsonProperty("tx")]
        public int X { get; set; }

        [JsonProperty("ty")]
        public int Y { get; set; }

        [JsonProperty("sp")]
        public int Speed { get; set; }

        [JsonProperty("intState")]
        public EntityState State
        {
            get => _state;
            set
            {
                if (value == EntityState.Dead && _state != EntityState.Dead)
                {
                    OnDeath();
                }

                _state = value;
            }
        }

        public bool Alive => State != EntityState.Dead;

        public abstract string TargetString { get; }
        public abstract int TargetId { get; }

        public event EntityDeathHandler Death;

        protected virtual void OnDeath()
        {
            Death?.Invoke(this);
        }
    }

    public interface IEntityBase : IState
    {
        string Name { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        int Mana { get; set; }
        int MaxMana { get; set; }
        int Level { get; set; }
        string Frame { get; set; }
        string Pad { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Speed { get; set; }
        EntityState State { get; set; }
        bool Alive { get; }
        string TargetString { get; }
        int TargetId { get; }

        event EntityDeathHandler Death;
    }
}
