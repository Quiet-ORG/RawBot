using RawBot.State;
using System.Threading.Tasks;

namespace RawBot.Network.Packets
{
    public interface IPacket
    {
        string Serialize();
        void Deserialize(string content);

        Task HandleAsync(Game game);
    }
}
