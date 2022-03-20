using RawBot.Logging;
using RawBot.Network;
using RawBot.Network.Packets;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RawBot.State
{
    public class Game
    {
        public Client Client { get; set; }

        public string Username { get; set; }
        public string Token { get; set; }
        public Options Options { get; } = new();
        public World World { get; private set; }
        public ILogger Log { get; set; } = new TextWriterLogger { Writer = Console.Out };

        public async Task ConnectAsync(string ip, int port)
        {
            Client = new(ip, port) { Log = Log };
            _ = Task.Run(Client.Start);
            World = new(this);

            Client.PacketReceived += ClientOnPacketReceived;
            await Client.SendAsync("<msg t='sys'><body action='verChk' r='0'><ver v='157' /></body></msg>");
        }

        public Task SendXtAsync(params object[] parts)
        {
            var packetSb = new StringBuilder().Append("%xt%");
            foreach (var part in parts)
            {
                if (part is not null && part.GetType().IsArray)
                {
                    foreach (var subpart in (Array)part)
                    {
                        packetSb.Append(subpart).Append('%');
                    }
                }
                else
                {
                    packetSb.Append(part).Append('%');
                }
            }

            return Client.SendAsync(packetSb.ToString());
        }

        private async void ClientOnPacketReceived(IPacket packet)
        {
            await packet.HandleAsync(this);
        }
    }
}
