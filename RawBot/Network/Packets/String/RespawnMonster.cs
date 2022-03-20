using RawBot.State;
using RawBot.State.Model.Entities;
using System.Threading.Tasks;

namespace RawBot.Network.Packets.String
{
    [Action("respawnMon")]
    public class RespawnMonster : StringPacket
    {
        public int Id => GetPart<int>(2);

        public override Task HandleAsync(Game game)
        {
            var monster = game.World.Monsters.Get(Id);
            if (monster is not null && monster.State == EntityState.Dead)
            {
                monster.State = EntityState.Alive;
            }

            return Task.CompletedTask;
        }
    }
}
