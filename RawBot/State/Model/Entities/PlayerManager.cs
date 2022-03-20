using RawBot.Utils;

namespace RawBot.State.Model.Entities
{
    public class PlayerManager : ListManager<IPlayer, int>
    {
        protected override int GetKey(IPlayer player)
        {
            return player.Id;
        }

        public IPlayer Get(string username)
        {
            return Find(p => p.Username.EqualsIgnoreCase(username));
        }
    }
}
