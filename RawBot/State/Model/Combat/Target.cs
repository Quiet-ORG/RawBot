using RawBot.State.Model.Entities;

namespace RawBot.State.Model.Combat
{
    public class Target
    {
        private volatile IEntityBase _target;

        public IEntityBase Entity
        {
            get => _target?.Alive ?? false ? _target : _target = null;
            set => _target = value;
        }

        public bool Exists => _target is not null;
    }
}
